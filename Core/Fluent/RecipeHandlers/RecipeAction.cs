namespace KitchenPC.Fluent.RecipeHandlers
{
    using System;
    using KitchenPC.Context;
    using KitchenPC.Context.Fluent;
    using KitchenPC.Recipes;
    using KitchenPC.Recipes.Enums;

    /// <summary>
    ///     Provides the ability to fluently express recipe related actions, such as loading recipes, finding recipes and
    ///     sharing recipes.
    /// </summary>
    public class RecipeAction
    {
        private readonly IKPCContext context;

        public RecipeAction(IKPCContext context)
        {
            this.context = context;
        }

        public IKPCContext Context
        {
            get
            {
                return this.context;
            }
        }

        public RecipeCreator Create
        {
            get
            {
                return new RecipeCreator(this.context);
            }
        }

        public RecipeLoader Load(Recipe recipe)
        {
            return new RecipeLoader(this.context, recipe);
        }

        public RecipeRater Rate(Recipe recipe, Rating rating)
        {
            return new RecipeRater(this.context, recipe, rating);
        }

        public RecipeFinder Search(RecipeQuery query)
        {
            return new RecipeFinder(this.context, query);
        }

        public RecipeFinder Search(Func<RecipeQueryBuilder, RecipeQueryBuilder> searchBuilder)
        {
            var builder = new RecipeQueryBuilder(new RecipeQuery());
            var query = searchBuilder(builder).Query;

            return this.Search(query);
        }
    }
}
