namespace KitchenPC.Fluent.RecipeHandlers
{
    using System;
    using KitchenPC.Context;
    using KitchenPC.Ingredients;
    using KitchenPC.Recipes;
    using KitchenPC.Recipes.Enums;

    /// <summary>Provides the ability to fluently build a search query.</summary>
    public class RecipeCreator
    {
        private readonly IKPCContext context;

        private readonly Recipe recipe;

        public RecipeCreator(IKPCContext context)
        {
            this.context = context;
            this.recipe = new Recipe();

            this.recipe.DateEntered = DateTime.Now;
        }

        public IKPCContext Context
        {
            get
            {
                return this.context;
            }
        }

        public Recipe Recipe
        {
            get
            {
                return this.recipe;
            }
        }

        public RecipeCreator SetTitle(string title)
        {
            this.recipe.Title = title;
            return this;
        }

        public RecipeCreator SetDescription(string desc)
        {
            this.recipe.Description = desc;
            return this;
        }

        public RecipeCreator SetCredit(string credit)
        {
            this.recipe.Credit = credit;
            return this;
        }

        public RecipeCreator SetCreditUrl(Uri creditUrl)
        {
            this.recipe.CreditUrl = creditUrl.ToString();
            return this;
        }

        public RecipeCreator SetMethod(string method)
        {
            this.recipe.Method = method;
            return this;
        }

        public RecipeCreator SetDateEntered(DateTime date)
        {
            this.recipe.DateEntered = date;
            return this;
        }

        public RecipeCreator SetPrepTime(short prepTime)
        {
            this.recipe.PrepTime = prepTime;
            return this;
        }

        public RecipeCreator SetCookTime(short cookTime)
        {
            this.recipe.CookTime = cookTime;
            return this;
        }

        public RecipeCreator SetRating(Rating rating)
        {
            this.recipe.UserRating = rating;
            return this;
        }

        public RecipeCreator SetServingSize(short servings)
        {
            this.recipe.ServingSize = servings;
            return this;
        }

        public RecipeCreator SetTags(RecipeTags tags)
        {
            this.recipe.Tags = tags;
            return this;
        }

        public RecipeCreator SetImage(Uri imageUri)
        {
            this.recipe.ImageUrl = imageUri.ToString();
            return this;
        }

        public RecipeCreator SetIngredients(IngredientUsage[] ingredients)
        {
            this.recipe.Ingredients = ingredients;
            return this;
        }

        public RecipeCreator SetIngredients(string section, IngredientUsage[] ingredients)
        {
            this.recipe.Ingredients = ingredients;
            this.recipe.Ingredients.ForEach(x => x.Section = section);
            return this;
        }

        public RecipeResult Commit()
        {
            return this.context.CreateRecipe(this.recipe);
        }
    }
}
