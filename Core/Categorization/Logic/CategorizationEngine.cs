namespace KitchenPC.Categorization.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KitchenPC.Categorization.Enums;
    using KitchenPC.Categorization.Interfaces;
    using KitchenPC.Categorization.Logic;
    using KitchenPC.Categorization.Models;
    using KitchenPC.Recipes;

    public class CategorizationEngine
    {
        private readonly Dictionary<Guid, IIngredientCommonality> commonIngredients;

        private readonly Analyzer analyzer;

        public CategorizationEngine(IDBLoader loader)
        {
            this.analyzer = new Analyzer(loader);
            this.commonIngredients = loader.LoadCommonIngredients().ToDictionary(icomonality => icomonality.IngredientId, icomonality => icomonality);
        }

        public CategorizationResult Categorize(Recipe recipe)
        {
            var result = new CategorizationResult();

            this.CategorizeMeal(recipe, result);
            this.CategorizeDiet(recipe, result);
            this.CategorizeNutrition(recipe, result);
            this.CategorizeSkill(recipe, result);
            this.CategorizeTaste(recipe, result);

            return result;
        }

        private void CategorizeMeal(Recipe recipe, CategorizationResult result)
        {
            IRecipeClassification trainedRecipe = this.analyzer.GetTrainedRecipe(recipe.Id);
            if (trainedRecipe != null)
            {
                result.MealBreakfast = trainedRecipe.IsBreakfast;
                result.MealLunch = trainedRecipe.IsLunch;
                result.MealDinner = trainedRecipe.IsDinner;
                result.MealDessert = trainedRecipe.IsDessert;
            }
            else
            {
                var analysis = this.analyzer.GetPrediction(recipe);

                result.MealBreakfast = analysis.FirstPlace.Equals(Category.Breakfast) || analysis.SecondPlace.Equals(Category.Breakfast);
                result.MealLunch = analysis.FirstPlace.Equals(Category.Lunch) || analysis.SecondPlace.Equals(Category.Lunch);
                result.MealDinner = analysis.FirstPlace.Equals(Category.Dinner) || analysis.SecondPlace.Equals(Category.Dinner);
                result.MealDessert = analysis.FirstPlace.Equals(Category.Dessert) || analysis.SecondPlace.Equals(Category.Dessert);
            }
        }

        private void CategorizeDiet(Recipe recipe, CategorizationResult result)
        {
            var ingredientMeta = recipe.Ingredients.Select(ing => ing.Ingredient.Metadata).ToArray();

            var glutenFree = ingredientMeta.All(ing => ing.HasGluten == false);
            var noAnimals = ingredientMeta.All(ing => ing.HasAnimal == false);
            var noMeat = ingredientMeta.All(ing => ing.HasMeat == false);
            var noPork = ingredientMeta.All(ing => ing.HasPork == false);
            var noRedMeat = ingredientMeta.All(ing => ing.HasRedMeat == false);

            result.DietGlutenFree = glutenFree;
            result.DietNoAnimals = noAnimals;
            result.DietNoMeat = noMeat;
            result.DietNoPork = noPork;
            result.DietNoRedMeat = noRedMeat;
        }

        private void CategorizeNutrition(Recipe recipe, CategorizationResult result)
        {
            float totalGrams = 0;
            float totalFat = 0;
            float totalSugar = 0;
            float totalCal = 0;
            float totalSodium = 0;
            float totalCarbs = 0;

            var noMatch = false;

            // First, convert every ingredient to weight
            foreach (var usage in recipe.Ingredients)
            {
                // No amount specified for this ingredient
                if (usage.Amount == null)
                {
                    noMatch = true;
                    continue;
                }

                var meta = usage.Ingredient.Metadata;

                if (meta == null)
                {
                    noMatch = true;
                    continue;
                }

                var amount = FormConversion.GetWeightForUsage(usage);
                if (amount == null)
                {
                    noMatch = true;
                    continue; // Cannot convert this ingredient to grams, skip it
                }

                var grams = amount.SizeHigh;
                totalGrams += grams;

                if (!(meta.FatPerUnit.HasValue && meta.SugarPerUnit.HasValue && meta.CaloriesPerUnit.HasValue
                      && meta.SodiumPerUnit.HasValue && meta.CarbsPerUnit.HasValue))
                {
                    noMatch = true;
                }
                else
                {
                    totalFat += (meta.FatPerUnit.Value * grams) / 100f; // Total fat per 100g
                    totalSugar += (meta.SugarPerUnit.Value * grams) / 100f; // Total sugar per 100g;
                    totalCal += (meta.CaloriesPerUnit.Value * grams) / 100f; // Total Calories per 100g
                    totalSodium += (meta.SodiumPerUnit.Value * grams) / 100f; // Total sodium per 100g             
                    totalCarbs += (meta.CarbsPerUnit.Value * grams) / 100f; // Total carbs per 100g
                }
            }

            result.USDAMatch = !noMatch; // Set to true if every ingredient has an exact USDA match

            // Set totals
            result.NutritionTotalFat = (short)totalFat;
            result.NutritionTotalSugar = (short)totalSugar;
            result.NutritionTotalCalories = (short)totalCal;
            result.NutritionTotalSodium = (short)totalSodium;
            result.NutritionTotalCarbs = (short)totalCarbs;

            // Flag RecipeMetadata depending on totals in recipe
            if (!noMatch)
            {
                result.NutritionLowFat = totalFat <= (totalCal * 0.03); // Definition of Low Fat is 3g of fat per 100 Cal
                result.NutritionLowSugar = totalSugar <= (totalCal * 0.02); // There is no FDA definition of "Low Sugar" (Can estimate 2g of sugar per 100 Cal or less)
                result.NutritionLowCalorie = totalCal <= (totalGrams * 1.2); // Definition of Low Calorie is 120 cal per 100g
                result.NutritionLowSodium = totalSodium <= (totalGrams * 1.4); // Definition of Low Sodium is 140mg per 100g
                result.NutritionLowCarb = totalCarbs <= (totalCal * 0.05); // No definition for Low Carb, but we can use 5g per 100 Cal or less
            }
        }

        private void CategorizeSkill(Recipe recipe, CategorizationResult result)
        {
            // Common: Has 3 or more ingredients and all ingredients are considered "common"
            result.SkillCommon = recipe.Ingredients.Length >= 3 && recipe.Ingredients.All(i => this.commonIngredients.ContainsKey(i.Ingredient.Id));
            result.Commonality = Convert.ToSingle(result.SkillCommon ? recipe.Ingredients.Average(i => this.commonIngredients[i.Ingredient.Id].Commonality) : 0f);

            // Easy: Has the word "easy" in the title, or (prep <= 15min and ingredients <= 5)
            result.SkillEasy = recipe.Title.ToLower().Contains("easy") || (recipe.PrepTime <= 15 && recipe.Ingredients.Length <= 5);

            // Quick: prep <= 10 and cooktime <= 20
            result.SkillQuick = recipe.PrepTime <= 10 && recipe.CookTime <= 20;
        }

        private void CategorizeTaste(Recipe recipe, CategorizationResult result)
        {
            var totalMass = new Amount(0, Units.Gram);
            float totalSweet = 0f, totalSpicy = 0f;
            foreach (var usage in recipe.Ingredients)
            {
                if (usage.Amount == null)
                {
                    continue;
                }

                if (usage.Ingredient.Metadata == null)
                {
                    continue;
                }

                var meta = usage.Ingredient.Metadata;

                var amt = FormConversion.GetWeightForUsage(usage);
                if (amt == null)
                {
                    continue;
                }

                totalMass += amt;
                totalSweet += amt.SizeHigh * meta.Sweet;
                totalSpicy += amt.SizeHigh * meta.Spicy;
            }

            if (totalMass.SizeHigh == 0)
            {
                return; // Nothing to calc, exit
            }

            var maxRating = totalMass.SizeHigh * 4;
            var recipeSweet = totalSweet / maxRating; // Pct sweet the recipe is
            var recipeSpicy = totalSpicy / maxRating; // Pct spicy the recipe is

            result.TasteSavoryToSweet = Convert.ToByte(recipeSweet * 100); // Scale in terms of percentage
            result.TasteMildToSpicy = Convert.ToByte(recipeSpicy * 100);
        }
    }
}