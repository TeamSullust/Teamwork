namespace KitchenPC.Categorization.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KitchenPC.Categorization.Enums;
    using KitchenPC.Categorization.Interfaces;
    using KitchenPC.Categorization.Models;
    using KitchenPC.Categorization.Models.Tokens;
    using KitchenPC.Recipes;

    public class Analyzer
    {
        private const float Tolerance = 0.05f;

        private const float SecondPlaceDifferenceTolerance = 0.8f;

        private float probability;

        private float invertedProbability;

        private RecipeIndex breakfastIndex;
        private RecipeIndex lunchIndex;
        private RecipeIndex dinnerIndex;
        private RecipeIndex dessertIndex;

        private Dictionary<Guid, IRecipeClassification> trainingData;

        public Analyzer(IDBLoader loader)
        {
            this.LoadTrainingData(loader);
        }

        public IRecipeClassification GetTrainedRecipe(Guid recipeId)
        {
            return this.trainingData.FirstOrDefault(x => x.Key == recipeId).Value;
        }

        public AnalyzerResult GetPrediction(Recipe recipe)
        {
            var rankBreakfast = new Ranking(Category.Breakfast);
            var rankLunch = new Ranking(Category.Lunch);
            var rankDinner = new Ranking(Category.Dinner);
            var rankDessert = new Ranking(Category.Dessert);

            // Setup Tournament
            this.Compete(recipe, this.breakfastIndex, this.lunchIndex, rankBreakfast, rankLunch, rankDinner, rankDessert);
            this.Compete(recipe, this.breakfastIndex, this.dinnerIndex, rankBreakfast, rankLunch, rankDinner, rankDessert);
            this.Compete(recipe, this.breakfastIndex, this.dessertIndex, rankBreakfast, rankLunch, rankDinner, rankDessert);
            this.Compete(recipe, this.lunchIndex, this.dinnerIndex, rankBreakfast, rankLunch, rankDinner, rankDessert);
            this.Compete(recipe, this.lunchIndex, this.dessertIndex, rankBreakfast, rankLunch, rankDinner, rankDessert);
            this.Compete(recipe, this.dinnerIndex, this.dessertIndex, rankBreakfast, rankLunch, rankDinner, rankDessert);

            // Choose winner
            var result = this.GetWinner(rankBreakfast, rankLunch, rankDinner, rankDessert);

            return result;
        }

        private void Compete(Recipe entry, RecipeIndex first, RecipeIndex second, Ranking rankBreakfast, Ranking rankLunch, Ranking rankDinner, Ranking rankDessert)
        {
            var chance = this.GetPrediction(entry, first, second);
            if (chance > 0.5f - Tolerance && chance < 0.5f + Tolerance)
            {
                return; // No winner
            }

            var diff = (float)Math.Abs(chance - 0.5);
            var winner = chance < 0.5 ? second : first;

            if (winner == this.breakfastIndex)
            {
                rankBreakfast.Score += diff;
            }

            if (winner == this.lunchIndex)
            {
                rankLunch.Score += diff;
            }

            if (winner == this.dinnerIndex)
            {
                rankDinner.Score += diff;
            }

            if (winner == this.dessertIndex)
            {
                rankDessert.Score += diff;
            }
        }

        private AnalyzerResult GetWinner(Ranking rankBreakfast, Ranking rankLunch, Ranking rankDinner, Ranking rankDessert)
        {
            var meals = new Ranking[] { rankBreakfast, rankLunch, rankDinner, rankDessert };
            var sorted = meals.OrderByDescending(m => m.Score).ToArray();

            var firstPlace = sorted[0];
            var difference = sorted[1].Score / sorted[0].Score;
            var secondPlace = difference > SecondPlaceDifferenceTolerance ? sorted[1] : null;

            var result = new AnalyzerResult(firstPlace.Type, secondPlace != null ? secondPlace.Type : Category.None);
            return result;
        }

        private float GetPrediction(Recipe recipe, RecipeIndex first, RecipeIndex second)
        {
            // Reset probability and invertedProbability
            this.invertedProbability = 0;
            this.probability = 0;

            var tokens = Tokenizer.Tokenize(recipe);

            foreach (var token in tokens)
            {
                var firstRITokensCount = first.GetTokenCount(token);
                var secondRITokensCount = second.GetTokenCount(token);

                this.CalcProbability(firstRITokensCount, first.EntryCount, secondRITokensCount, second.EntryCount);
            }

            var prediction = this.probability / (this.probability + this.invertedProbability);
            return prediction;
        }

        private void CalcProbability(float firstRITokensCount, float firstRITotalTokens, float secondRITokensCount, float secondRITotalTokens)
        {
            const float DivisorConstant = 1f;
            const float DividentConstant = .5f;

            var firstRIProbability = firstRITokensCount / firstRITotalTokens;
            var secondRIProbability = secondRITokensCount / secondRITotalTokens;
            var firstComparedToCombinedProbability = firstRIProbability / (firstRIProbability + secondRIProbability);
            var combinedRITokens = firstRITokensCount + secondRITokensCount;
            var prob = (DividentConstant + (combinedRITokens * firstComparedToCombinedProbability)) / (DivisorConstant + combinedRITokens);

            if (this.probability == 0)
            {
                this.probability = prob;
            }
            else
            {
                this.probability = this.probability * prob;
            }

            if (this.invertedProbability == 0)
            {
                this.invertedProbability = 1 - prob;
            }
            else
            {
                this.invertedProbability = this.invertedProbability * (1 - prob);
            }
        }

        private void LoadTrainingData(IDBLoader loader)
        {
            this.trainingData = new Dictionary<Guid, IRecipeClassification>();

            this.breakfastIndex = new RecipeIndex();
            this.lunchIndex = new RecipeIndex();
            this.dinnerIndex = new RecipeIndex();
            this.dessertIndex = new RecipeIndex();

            var data = loader.LoadTrainingData();

            foreach (var recipeClassification in data)
            {
                this.trainingData.Add(recipeClassification.Recipe.Id, recipeClassification);

                if (recipeClassification.IsBreakfast)
                {
                    this.breakfastIndex.Add(recipeClassification.Recipe);
                }
                if (recipeClassification.IsLunch)
                {
                    this.lunchIndex.Add(recipeClassification.Recipe);
                }
                if (recipeClassification.IsDinner)
                {
                    this.dinnerIndex.Add(recipeClassification.Recipe);
                }
                if (recipeClassification.IsDessert)
                {
                    this.dessertIndex.Add(recipeClassification.Recipe);
                }
            }
        }
    }
}