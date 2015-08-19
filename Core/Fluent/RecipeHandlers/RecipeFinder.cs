namespace KitchenPC.Fluent.RecipeHandlers
{
    using KitchenPC.Context;
    using KitchenPC.Recipes;

    /// <summary>Provides the ability to search for recipe.</summary>
    public class RecipeFinder
    {
        private readonly IKPCContext context;

        private readonly RecipeQuery query;

        public RecipeFinder(IKPCContext context, RecipeQuery query)
        {
            this.context = context;
            this.query = query;
        }

        public IKPCContext Context
        {
            get
            {
                return this.context;
            }
        }

        public RecipeQuery Query
        {
            get
            {
                return this.query;
            }
        }

        public SearchResults Results()
        {
            return this.context.RecipeSearch(this.query);
        }
    }
}
