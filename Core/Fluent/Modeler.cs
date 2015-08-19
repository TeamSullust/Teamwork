namespace KitchenPC.Fluent
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KitchenPC.Context;
    using KitchenPC.Ingredients;
    using KitchenPC.Modeler;
    using KitchenPC.Recipes;

    using IngredientUsage = KitchenPC.Ingredients.IngredientUsage;

    /// <summary>Provides the ability to fluently express modeler related actions, such as generating or compiling a model.</summary>
    public class ModelerAction
    {
        private readonly IKPCContext context;

        public ModelerAction(IKPCContext context)
        {
            this.context = context;
        }

        public ModelingSessionAction WithAnonymous
        {
            get
            {
                return new ModelingSessionAction(this.context, UserProfile.Anonymous);
            }
        }

        public ModelingSessionAction WithSession(ModelingSession session)
        {
            return new ModelingSessionAction(session);
        }

        public ModelingSessionAction WithProfile(IUserProfile profile)
        {
            return new ModelingSessionAction(this.context, profile);
        }

        public ModelingSessionAction WithProfile(Func<ProfileCreator, ProfileCreator> profileCreator)
        {
            var creator = profileCreator(new ProfileCreator());

            return new ModelingSessionAction(this.context, creator.Profile);
        }
    }

    public class ProfileCreator
    {
        private readonly IList<Guid> blacklistIng;

        private readonly IList<Guid> favIngs;

        private readonly IList<PantryItem> pantry;

        private readonly IList<RecipeRating> ratings;

        private RecipeTags allowedTags;

        private Guid avoidRecipe;

        private RecipeTags favTags;

        private Guid userid;

        public ProfileCreator()
        {
            this.ratings = new List<RecipeRating>();
            this.pantry = new List<PantryItem>();
            this.favIngs = new List<Guid>();
            this.blacklistIng = new List<Guid>();
        }

        public IUserProfile Profile
        {
            get
            {
                return new UserProfile
                           {
                               UserId = this.userid, 
                               Ratings = this.ratings.ToArray(), 
                               Pantry = this.pantry.Any() ? this.pantry.ToArray() : null, 

                               // Pantry must be null or more than 0 items
                               FavoriteIngredients = this.favIngs.ToArray(), 
                               FavoriteTags = this.favTags, 
                               BlacklistedIngredients = this.blacklistIng.ToArray(), 
                               AvoidRecipe = this.avoidRecipe, 
                               AllowedTags = this.allowedTags
                           };
            }
        }

        public ProfileCreator WithUserId(Guid userid)
        {
            this.userid = userid;
            return this;
        }

        public ProfileCreator AddRating(RecipeRating rating)
        {
            this.ratings.Add(rating);
            return this;
        }

        public ProfileCreator AddRating(Recipe recipe, byte rating)
        {
            this.ratings.Add(new RecipeRating { RecipeId = recipe.Id, Rating = rating });

            return this;
        }

        public ProfileCreator AddPantryItem(PantryItem item)
        {
            this.pantry.Add(item);
            return this;
        }

        public ProfileCreator AddPantryItem(IngredientUsage usage)
        {
            this.pantry.Add(new PantryItem(usage));
            return this;
        }

        public ProfileCreator AddFavoriteIngredient(Ingredient ingredient)
        {
            this.favIngs.Add(ingredient.Id);
            return this;
        }

        public ProfileCreator FavoriteTags(RecipeTags tags)
        {
            this.favTags = tags;
            return this;
        }

        public ProfileCreator AddBlacklistedIngredient(Ingredient ingredient)
        {
            this.blacklistIng.Add(ingredient.Id);
            return this;
        }

        public ProfileCreator AvoidRecipe(Recipe recipe)
        {
            this.avoidRecipe = recipe.Id;
            return this;
        }

        public ProfileCreator AllowedTags(RecipeTags tags)
        {
            this.allowedTags = tags;
            return this;
        }
    }

    public class ModelingSessionAction
    {
        private readonly ModelingSession session;

        private int recipes = 5;

        private byte scale = 2;

        public ModelingSessionAction(ModelingSession session)
        {
            this.session = session;
        }

        public ModelingSessionAction(IKPCContext context, IUserProfile profile)
        {
            this.session = context.CreateModelingSession(profile);
        }

        public ModelingSessionAction NumRecipes(int recipes)
        {
            this.recipes = recipes;
            return this;
        }

        public ModelingSessionAction Scale(byte scale)
        {
            this.scale = scale;
            return this;
        }

        public Model Generate()
        {
            return this.session.Generate(this.recipes, this.scale);
        }

        public CompiledModel Compile()
        {
            var model = this.Generate();

            return this.session.Compile(model);
        }
    }
}