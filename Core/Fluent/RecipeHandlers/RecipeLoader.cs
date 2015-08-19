namespace KitchenPC.Fluent.RecipeHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using KitchenPC.Context;
    using KitchenPC.Recipes;

    /// <summary>Provides the ability to load one or more recipes.</summary>
    public class RecipeLoader
    {
        private readonly IKPCContext context;

        private readonly IList<Recipe> recipesToLoad;

        public RecipeLoader(IKPCContext context, Recipe recipe)
        {
            this.context = context;
            this.recipesToLoad = new List<Recipe> { recipe };
        }

        public IKPCContext Context
        {
            get
            {
                return this.context;
            }
        }

        public IList<Recipe> RecipesToLoad
        {
            get
            {
                return new List<Recipe>(this.recipesToLoad);
            }
        }

        public bool WithCommentCount { get; set; }

        public bool WithCookbookStatus { get; set; }

        public bool WithMethod { get; set; }

        public bool WithPermalink { get; set; }

        public bool WithUserRating { get; set; }

        public RecipeLoader SetCommentCountLoading()
        {

            this.WithCommentCount = true;
            return this;

        }

        public RecipeLoader SetUserRatingLoading()
        {

            this.WithUserRating = true;
            return this;

        }

        public RecipeLoader SetCookbookStatusLoading()
        {

            this.WithCookbookStatus = true;
            return this;

        }

        public RecipeLoader SetMethodLoading()
        {

            this.WithMethod = true;
            return this;

        }

        public RecipeLoader SetPermalinkLoading()
        {

            this.WithPermalink = true;
            return this;

        }

        public RecipeLoader Load(Recipe recipe)
        {
            this.recipesToLoad.Add(recipe);
            return this;
        }

        public IList<Recipe> List()
        {
            var options = new ReadRecipeOptions
            {
                ReturnCommentCount = this.WithCommentCount,
                ReturnCookbookStatus = this.WithCookbookStatus,
                ReturnMethod = this.WithMethod,
                ReturnPermalink = this.WithPermalink,
                ReturnUserRating = this.WithUserRating
            };

            return this.context.ReadRecipes(this.recipesToLoad.Select(r => r.Id).ToArray(), options);
        }
    }
}
