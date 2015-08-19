namespace KitchenPC.Modeler
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using KitchenPC.Context;
    using KitchenPC.Recipes;
    using KitchenPC.Recipes.Enums;

    using log4net;

    /// <summary>
    /// Represents a modeling session for a given user with a given pantry.
    /// This object can be re-used (or cached) while the user changes modeling preferences and remodels.
    /// </summary>
    public class ModelingSession
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof(ModelingSession));

        private const int MaxSuggestions = 15;
        private const double CoolingRate = 0.9999;
        private const float MissingIngPunish = 5.0f;
        private const float NewIngPunish = 2.0f;
        private const float EmptyRecipeAmount = 0.50f;    

        private readonly Random random = new Random();
        private readonly IngredientNode[] pantryIngredients;
        private readonly Dictionary<IngredientNode, float?> pantryAmounts;
        private readonly List<IngredientNode> ingBlacklist;
        private readonly Dictionary<RecipeNode, byte> ratings;

        private readonly bool[] favTags; // Truth table of fav tags
        private readonly int[] favIngs; // Array of top 5 fav ings by id

        private readonly RecipeTags allowedTags; // Copy of profile     

        private readonly DBSnapshot db;

        private readonly IKPCContext context;

        private readonly IUserProfile profile;

        private Dictionary<IngredientNode, IngredientUsage> totals; // Hold totals for each scoring round so we don't have to reallocate map every time

        /// <summary>
        /// Create a ModelingSession instance.
        /// </summary>
        /// <param name="context">KitchenPC context used for this modeling session.</param>
        /// <param name="db">Object containing all available recipes, ratings, and trend information.</param>
        /// <param name="profile">Object containing user specific information, such as pantry and user ratings.</param>
        public ModelingSession(IKPCContext context, DBSnapshot db, IUserProfile profile)
        {
            this.db = db;
            this.context = context;
            this.profile = profile;
            this.favTags = new bool[Enum.GetNames(typeof(RecipeTag)).Length];
            this.favIngs = new int[profile.FavoriteIngredients.Length];

            if (profile.Pantry != null && profile.Pantry.Length == 0) 
            {
                // Empty pantries must be null, not zero items
                throw new EmptyPantryException();
            }

            if (profile.AllowedTags != null)
            {
                this.allowedTags = profile.AllowedTags;
            }

            if (profile.Pantry != null)
            {
                this.pantryAmounts = new Dictionary<IngredientNode, float?>();
                foreach (var item in profile.Pantry)
                {
                    var node = this.db.FindIngredient(item.IngredientId);

                    // If an ingredient isn't used by any recipe, there's no reason for it to be in the pantry.
                    if (node == null)
                    {
                        continue;
                    }

                    // If an ingredient exists, but doesn't have any link to any allowed tags, there's no reason for it to be in the pantry.
                    if (this.allowedTags != null && (!node.AvailableTags.Intersect(this.allowedTags).Any()))
                    {
                        continue;
                    }

                    if (this.pantryAmounts.ContainsKey(node))
                    {
                        throw new DuplicatePantryItemException();
                    }

                    this.pantryAmounts.Add(node, item.Amt);
                }

                if (this.pantryAmounts.Keys.Count == 0)
                {
                    throw new ImpossibleQueryException();
                }

                this.pantryIngredients = this.pantryAmounts.Keys.ToArray();
            }

            if (profile.FavoriteIngredients != null)
            {
                var i = 0;
                foreach (var ing in profile.FavoriteIngredients)
                {
                    var node = this.db.FindIngredient(ing);
                    this.favIngs[i] = node.Key;
                }
            }

            if (profile.FavoriteTags != null)
            {
                foreach (var tag in profile.FavoriteTags)
                {
                    this.favTags[(int)tag] = true;
                }
            }

            if (profile.BlacklistedIngredients != null)
            {
                this.ingBlacklist = new List<IngredientNode>();
                foreach (var ing in profile.BlacklistedIngredients)
                {
                    var node = this.db.FindIngredient(ing);
                    this.ingBlacklist.Add(node);
                }
            }

            if (profile.Ratings != null)
            {
                this.ratings = new Dictionary<RecipeNode, byte>(profile.Ratings.Length);
                foreach (var r in profile.Ratings)
                {
                    var n = this.db.FindRecipe(r.RecipeId);
                    this.ratings.Add(n, r.Rating);
                }
            }
            else
            {
                this.ratings = new Dictionary<RecipeNode, byte>(0);
            }
        }

        /// <summary>
        /// Generates a model with the specified number of recipes and returns the recipe IDs in the optimal order.
        /// </summary>
        /// <param name="recipes">Number of recipes to generate</param>
        /// <param name="scale">Scale indicating importance of optimal ingredient usage vs. user trend usage. 1 indicates ignore user trends, return most efficient set of recipes. 5 indicates ignore pantry and generate recipes user is most likely to rate high.</param>
        /// <returns>An array up to size "recipes" containing recipes from DBSnapshot.</returns>
        public Model Generate(int recipes, byte scale)
        {
            if (recipes > MaxSuggestions)
            {
                throw new ArgumentException("Modeler can only generate " + MaxSuggestions + " recipes at a time.");
            }

            var temperature = 10000.0;
            const double AbsoluteTemperature = 0.00001;

            this.totals = new Dictionary<IngredientNode, IngredientUsage>(IngredientNode.NextKey);

            var currentSet = new RecipeNode[recipes]; // current set of recipes
            this.InitSet(currentSet); // Initialize with n random recipes
            var score = this.GetScore(currentSet, scale); // Check initial score

            var timer = new Stopwatch();
            timer.Start();

            while (temperature > AbsoluteTemperature)
            {
                var nextSet = this.GetNextSet(currentSet); // set to compare with current
                var deltaScore = this.GetScore(nextSet, scale) - score;

                // if the new set has a smaller score (good thing)
                // or if the new set has a higher score but satisfies Boltzman condition then accept the set
                if ((deltaScore < 0) || (score > 0 && Math.Exp(-deltaScore / temperature) > this.random.NextDouble()))
                {
                    nextSet.CopyTo(currentSet, 0);
                    score += deltaScore;
                }

                // cool down the temperature
                temperature *= CoolingRate;
            }

            timer.Stop();
            Log.InfoFormat("Generating set of {0} recipes took {1}ms.", recipes, timer.ElapsedMilliseconds);

            return new Model(currentSet, this.profile.Pantry, score);
        }

        /// <summary>Takes a model generated from the modeling engine and loads necessary data from the database to deliver relevance to a user interface.</summary>
        /// <param name="model">Model from modeling engine</param>
        /// <returns>CompiledModel object which contains full recipe information about the provided set.</returns>
        public CompiledModel Compile(Model model)
        {
            var results = new CompiledModel();

            var recipes = this.context.ReadRecipes(model.RecipeIds, new ReadRecipeOptions());

            results.RecipeIds = model.RecipeIds;
            results.Pantry = model.Pantry;
            results.Briefs = recipes.Select(r => new RecipeBrief(r)).ToArray();
            results.Recipes = recipes.Select(r => new SuggestedRecipe
            {
                Id = r.Id,
                Ingredients = this.context.AggregateRecipes(r.Id).ToArray()
            }).ToArray();

            return results;
        }

        /// <summary>
        /// Judges a set of recipes based on a scale and its efficiency with regards to the current pantry.  The lower the score, the better.
        /// </summary>
        public double GetScore(RecipeNode[] currentSet, byte scale)
        {
            double wasted = 0; // Add 1.0 for ingredients that don't exist in pantry, add percentage of leftover otherwise
            float avgRating = 0; // Average rating for all recipes in the set (0-4)
            float tagPoints = 0; // Point for each tag that's one of our favorites
            float tagTotal = 0; // Total number of tags in all recipes
            float ingPoints = 0; // Point for each ing that's one of our favorites
            float ingTotal = 0; // Total number of ingrediets in all recipes

            for (var i = 0; i < currentSet.Length; i++)
            {
                var recipe = currentSet[i];
                var ingredients = (IngredientUsage[])recipe.Ingredients;

                // Add points for any favorite tags this recipe uses
                tagTotal += recipe.Tags.Length;
                ingTotal += ingredients.Length;

                for (var t = 0; t < recipe.Tags.Length; t++)
                {
                    if (this.favTags[t])
                    {
                        tagPoints++;
                    }
                }

                byte realRating; // Real rating is the user's rating, else the public rating, else 3.
                if (!this.ratings.TryGetValue(recipe, out realRating))
                {
                    realRating = (recipe.Rating == 0) ? (byte)3 : recipe.Rating; // if recipe has no ratings, use average rating of 3.
                }
                avgRating += realRating - 1;

                foreach (var ingredient in ingredients)
                {
                    // Add points for any favorite ingredients this recipe uses
                    var ingKey = ingredient.Ingredient.Key;

                    if (this.favIngs.Any(t => t == ingKey))
                    {
                        ingPoints++;
                    }

                    IngredientUsage curUsage;
                    var fContains = this.totals.TryGetValue(ingredient.Ingredient, out curUsage);
                    if (!fContains)
                    {
                        curUsage = new IngredientUsage();
                        curUsage.Amt = ingredient.Amt;
                        this.totals.Add(ingredient.Ingredient, curUsage);
                    }
                    else
                    {
                        curUsage.Amt += ingredient.Amt;
                    }
                }
            }

            // If profile has a pantry, figure out how much of it is wasted
            if (this.profile.Pantry != null) 
            {
                // For each pantry ingredient that we're not using, punish the score by MISSING_ING_PUNISH amount.
                var pEnum = this.pantryAmounts.GetEnumerator();
                while (pEnum.MoveNext())
                {
                    if (!this.totals.ContainsKey(pEnum.Current.Key))
                    {
                        wasted += MissingIngPunish;
                    }
                }

                var e = this.totals.GetEnumerator();
                while (e.MoveNext())
                {
                    var curKey = e.Current.Key;

                    float? have;

                    // We have this in our pantry
                    if (this.pantryAmounts.TryGetValue(curKey, out have)) 
                    {
                        // We have this in our pantry, but no amount is specified - So we "act" like we have whatever we need
                        if (!have.HasValue) 
                        {
                            continue;
                        }

                        // This recipe doesn't specify an amount - So we "act" like we use half of what we have
                        if (!e.Current.Value.Amt.HasValue) 
                        {
                            wasted += EmptyRecipeAmount;
                            continue;
                        }

                        var need = e.Current.Value.Amt.Value;
                        var ratio = 1 - ((have.Value - need) / have.Value); // Percentage of how much you're using of what you have
                        // If you need more than you have, add the excess ratio to the waste but don't go over the punishment for not having the ingredient at all
                        if (ratio > 1) 
                        {
                            wasted += Math.Min(ratio, NewIngPunish);
                        }
                        else
                        {
                            wasted += 1 - ratio;
                        }
                    }
                    else
                    {
                        wasted += NewIngPunish; // For each ingredient this meal set needs that we don't have, increment by NEW_ING_PUNISH
                    }
                }
            }

            double efficiencyScore;

            // No pantry, efficiency is defined by the overlap of ingredients across recipes
            if (this.profile.Pantry == null) 
            {
                efficiencyScore = totals.Keys.Count / ingTotal;
            }
            else      
            {
                // Efficiency is defined by how efficient the pantry ingredients are utilized
                double worstScore = (this.totals.Keys.Count * NewIngPunish) + (this.profile.Pantry.Length * MissingIngPunish);
                efficiencyScore = wasted / worstScore;
            }

            avgRating /= currentSet.Length;
            double trendScore = 1 - ((((avgRating / 4) * 4) + (tagPoints / tagTotal) + (ingPoints / ingTotal)) / 6);

            this.totals.Clear();

            switch (scale)
            {
                case 1:
                    return efficiencyScore;
                case 2:
                    return (efficiencyScore + efficiencyScore + trendScore) / 3;
                case 3:
                    return (efficiencyScore + trendScore) / 2;
                case 4:
                    return (efficiencyScore + trendScore + trendScore) / 3;
                case 5:
                    return trendScore;
            }

            return 0;
        }

        /// <summary>
        /// Initializes currentSet with random recipes from the available recipe pool.
        /// </summary>
        public void InitSet(RecipeNode[] currentSet)
        {
            var inUse = new List<Guid>(currentSet.Length);

            for (var i = 0; i < currentSet.Length; i++)
            {
                RecipeNode g;
                var timeout = 0;
                do
                {
                    g = this.Fish();

                    // Ok we've tried 100 times to find a recipe not already in this set, there just isn't enough data to work with for this query
                    if (++timeout > 100)
                    {
                        // TODO: Maybe we can lower the demanded meals and return what we do have
                        throw new ImpossibleQueryException();
                           
                    }
                }
                while (inUse.Contains(g.RecipeId));

                inUse.Add(g.RecipeId);
                currentSet[i] = g;
            }
        }

        /// <summary>
        /// Swap out a random recipe with another one from the available pool
        /// </summary>
        private RecipeNode[] GetNextSet(RecipeNode[] currentSet)
        {
            var rndIndex = this.random.Next(currentSet.Length);
            var existingRecipe = currentSet[rndIndex];
            RecipeNode newRecipe;

            var timeout = 0;
            while (true)
            {
                // We've tried 100 times to replace a recipe in this set, but cannot find anything that isn't already in this set.
                if (++timeout > 100) 
                {
                    // TODO: If this is the only set of n which match that query, we've solved it - just return this set as final!
                    throw new ImpossibleQueryException(); 
                }

                newRecipe = this.Fish();
                if (newRecipe == existingRecipe)
                {
                    continue;
                }

                var fFound = currentSet.Any(t => newRecipe == t);

                if (!fFound)
                {
                    break;
                }
            }

            var retSet = new RecipeNode[currentSet.Length];
            currentSet.CopyTo(retSet, 0);
            retSet[rndIndex] = newRecipe;

            return retSet;
        }

        /// <summary>
        /// Finds a random recipe in the available recipe pool
        /// </summary>
        /// <returns></returns>
        private RecipeNode Fish()
        {
            RecipeNode recipeNode = null;

            // No pantry, fish through Recipe index
            if (this.pantryIngredients == null) 
            {
                var tag = (this.allowedTags == null) ? this.random.Next(Enum.GetNames(typeof(RecipeTag)).Length) : (int)this.allowedTags[this.random.Next(this.allowedTags.Length)];
                var recipesByTag = this.db.FindRecipesByTag(tag);

                // Nothing in that tag
                if (recipesByTag == null || recipesByTag.Length == 0) 
                {
                    return Fish();
                }

                var rnd = this.random.Next(recipesByTag.Length);
                recipeNode = recipesByTag[rnd];
            }
            else
            {
                // Fish through random pantry ingredient
                var rndIng = this.random.Next(this.pantryIngredients.Length);
                var ingNode = this.pantryIngredients[rndIng];

                RecipeNode[] recipes;
                if (this.allowedTags != null && this.allowedTags.Length > 0)
                {
                    // Does this ingredient have any allowed tags?
                    if (!this.allowedTags.Intersect(ingNode.AvailableTags).Any()) 
                    {
                        return this.Fish(); // No - Find something else
                    }

                    // Pick random tag from allowed tags (since this set is smaller, better to guess an available tag)
                    while (true)
                    {
                        var t = this.random.Next(this.allowedTags.Length); // NOTE: Next will NEVER return MaxValue, so we don't subtract 1 from Length!
                        var rndTag = this.allowedTags[t];
                        recipes = ingNode.RecipesByTag[(int)rndTag] as RecipeNode[];

                        if (recipes != null)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    // Just pick a random available tag
                    var rndTag = this.random.Next(ingNode.AvailableTags.Length);
                    var tag = ingNode.AvailableTags[rndTag];
                    recipes = ingNode.RecipesByTag[(int)tag] as RecipeNode[];
                }

                if (recipes != null)
                {
                    var rndRecipe = this.random.Next(recipes.Length);
                    recipeNode = recipes[rndRecipe];
                }
            }

            // If there's a blacklist, make sure no ingredients are blacklisted otherwise try again
            if (this.ingBlacklist != null)
            {
                if (recipeNode != null)
                {
                    var ingredients = (IngredientUsage[])recipeNode.Ingredients;
                    for (var i = 0; i < ingredients.Length; i++)
                    {
                        if (this.ingBlacklist.Contains(ingredients[i].Ingredient))
                        {
                            return this.Fish();
                        }
                    }
                }
            }

            // Discard if this recipe is to be avoided
            if (recipeNode != null && (this.profile.AvoidRecipe.HasValue && this.profile.AvoidRecipe.Value.Equals(recipeNode.RecipeId)))
            {
                return this.Fish();
            }

            return recipeNode;
        }
    }
}