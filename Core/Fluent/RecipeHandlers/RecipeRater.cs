namespace KitchenPC.Fluent.RecipeHandlers
{
    using System.Collections.Generic;
    using KitchenPC.Context;
    using KitchenPC.Recipes;
    using KitchenPC.Recipes.Enums;

    /// <summary>Provides the ability to rate a recipe.</summary>
    public class RecipeRater
    {
        private readonly IKPCContext context;

        private readonly Dictionary<Recipe, Rating> newRatings;

        public RecipeRater(IKPCContext context, Recipe recipe, Rating rating)
        {
            this.context = context;
            this.newRatings = new Dictionary<Recipe, Rating>();

            this.newRatings.Add(recipe, rating);
        }

        public IKPCContext Context
        {
            get
            {
                return this.context;
            }
        }

        public Dictionary<Recipe, Rating> NewRatings
        {
            get
            {
                return new Dictionary<Recipe, Rating>(this.newRatings);
            }
        }

        public RecipeRater Rate(Recipe recipe, Rating rating)
        {
            this.newRatings.Add(recipe, rating);
            return this;
        }

        public void Commit()
        {
            foreach (var newRating in this.newRatings)
            {
                this.context.RateRecipe(newRating.Key.Id, newRating.Value);
            }
        }
    }
}
