namespace KitchenPC.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using KitchenPC.Context.Fluent;
    using KitchenPC.Fluent.RecipeHandlers;
    using KitchenPC.Ingredients;
    using KitchenPC.Modeler;
    using KitchenPC.Recipes;
    using KitchenPC.Recipes.Enums;
    using KitchenPC.Recipes.Filters;

    using KPCServer.UnitTests;

    using NUnit.Framework;

    using IngredientUsage = KitchenPC.Ingredients.IngredientUsage;

    [TestFixture]
    public class FluentRecipesTests
    {
        protected MockContext Context { get; private set; }

        protected RecipeAction Action { get; private set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            this.Context = new MockContext();
            this.Action = new RecipeAction(this.Context);
        }

        // RecipeAction Tests
        [Test]
        public void RecipeActionConstructor_ShouldReturnAnObjectWithCorrectValues()
        {
            Assert.AreSame(this.Context, this.Action.Context);
        }

        [Test]
        public void RecipeActionCreate_ShouldReturnARecipeCreatorWithCorrectValues()
        {
            var recipeCreator = this.Action.Create;

            Assert.IsInstanceOf(typeof(RecipeCreator), recipeCreator);
            Assert.AreSame(this.Context, recipeCreator.Context);
        }

        [Test]
        public void RecipeActionLoad_ShouldReturnARecipeLoaderWithCorrectValues()
        {
            var recipe = Mock.Recipes.SWEET_AND_SPICY_CHICKEN;

            var recipeLoader = this.Action.Load(recipe);

            Assert.IsInstanceOf(typeof(RecipeLoader), recipeLoader);
            Assert.AreEqual(1, recipeLoader.RecipesToLoad.Count);
            Assert.AreSame(this.Context, recipeLoader.Context);
            Assert.AreSame(recipe, recipeLoader.RecipesToLoad[0]);
        }

        [Test]
        public void RecipeActionRate_ShouldReturnARecipeRaterWithCorrectValues()
        {
            var recipe = Mock.Recipes.SWEET_AND_SPICY_CHICKEN;

            var recipeRater = this.Action.Rate(recipe, Rating.FiveStars);

            Assert.IsInstanceOf(typeof(RecipeRater), recipeRater);
            Assert.AreEqual(1, recipeRater.NewRatings.Count);
            Assert.IsTrue(recipeRater.NewRatings.ContainsKey(recipe));
            Assert.AreSame(this.Context, recipeRater.Context);
            Assert.AreEqual(Rating.FiveStars, recipeRater.NewRatings[recipe]);
        }

        [Test]
        public void RecipeActionSearch_ShouldReturnARecipeFinderWithCorrectValues()
        {
            var recipeQuery = new RecipeQuery();

            var recipeFinder = this.Action.Search(recipeQuery);

            Assert.IsInstanceOf(typeof(RecipeFinder), recipeFinder);
            Assert.AreSame(this.Context, recipeFinder.Context);
            Assert.AreSame(recipeQuery, recipeFinder.Query);
        }

        // RecipeCreator Tests
        [Test]
        public void RecipeCreatorSetTitle_ShouldSetRecipeTitle()
        {
            var recipeCreator = new RecipeCreator(this.Context);
            var title = "Title";
            recipeCreator.SetTitle(title);

            Assert.AreEqual(title, recipeCreator.Recipe.Title);
        }

        [Test]
        public void RecipeCreatorSetDescription_ShouldSetRecipeDescription()
        {
            var recipeCreator = new RecipeCreator(this.Context);
            var description = "Description";
            recipeCreator.SetDescription(description);

            Assert.AreEqual(description, recipeCreator.Recipe.Description);
        }

        [Test]
        public void RecipeCreatorSetCredit_ShouldSetRecipeCredit()
        {
            var recipeCreator = new RecipeCreator(this.Context);
            var credit = "https://www.google.com";
            recipeCreator.SetCredit(credit);

            Assert.AreEqual(credit, recipeCreator.Recipe.Credit);
        }

        [Test]
        public void RecipeCreatorSetCreditUrl_ShouldSetRecipeCreditUrl()
        {
            var recipeCreator = new RecipeCreator(this.Context);
            var creditUrl = "https://www.google.com";
            recipeCreator.SetCreditUrl(new Uri(creditUrl));

            Assert.AreEqual(creditUrl + "/", recipeCreator.Recipe.CreditUrl);
        }

        [Test]
        public void RecipeCreatorSetMethod_ShouldSetRecipeMethod()
        {
            var recipeCreator = new RecipeCreator(this.Context);
            var method = "Method";
            recipeCreator.SetMethod(method);

            Assert.AreEqual(method, recipeCreator.Recipe.Method);
        }

        [Test]
        public void RecipeCreatorSetDate_ShouldSetRecipeDate()
        {
            var recipeCreator = new RecipeCreator(this.Context);
            var date = new DateTime(2000, 6, 6);
            recipeCreator.SetDateEntered(date);

            Assert.AreEqual(date, recipeCreator.Recipe.DateEntered);
        }

        [Test]
        public void RecipeCreatorSetPrepTime_ShouldSetRecipePrepTime()
        {
            var recipeCreator = new RecipeCreator(this.Context);
            short prepTime = 10;
            recipeCreator.SetPrepTime(prepTime);

            Assert.AreEqual(prepTime, recipeCreator.Recipe.PrepTime);
        }

        [Test]
        public void RecipeCreatorSetCookTime_ShouldSetRecipeCookTime()
        {
            var recipeCreator = new RecipeCreator(this.Context);
            short cookTime = 10;
            recipeCreator.SetCookTime(cookTime);

            Assert.AreEqual(cookTime, recipeCreator.Recipe.CookTime);
        }

        [Test]
        public void RecipeCreatorSetRating_ShouldSetRecipeRating()
        {
            var recipeCreator = new RecipeCreator(this.Context);
            var rating = Rating.FiveStars;
            recipeCreator.SetRating(rating);

            Assert.AreEqual(rating, recipeCreator.Recipe.UserRating);
        }

        [Test]
        public void RecipeCreatorSetServingSize_ShouldSetRecipeServingSize()
        {
            var recipeCreator = new RecipeCreator(this.Context);
            short servingSize = 4;
            recipeCreator.SetServingSize(servingSize);

            Assert.AreEqual(servingSize, recipeCreator.Recipe.ServingSize);
        }

        [Test]
        public void RecipeCreatorSetTags_ShouldSetRecipeTags()
        {
            var recipeCreator = new RecipeCreator(this.Context);
            var tags = new RecipeTags(RecipeTag.CommonIngredients);
            recipeCreator.SetTags(tags);

            CollectionAssert.AreEqual(tags, recipeCreator.Recipe.Tags);
        }

        [Test]
        public void RecipeCreatorSetImage_ShouldSetRecipeImage()
        {
            var recipeCreator = new RecipeCreator(this.Context);
            var image = "https://www.image.com";
            recipeCreator.SetImage(new Uri(image));

            Assert.AreEqual(image + "/", recipeCreator.Recipe.ImageUrl);
        }

        [Test]
        public void RecipeCreatorSetIngredients_ShouldSetRecipeIngredients()
        {
            var recipeCreator = new RecipeCreator(this.Context);
            var recipeCreator2 = new RecipeCreator(this.Context);
            var ingredients = new IngredientUsage[] { new IngredientUsage() };
            var section = "Section";

            recipeCreator.SetIngredients(ingredients);
            recipeCreator2.SetIngredients(section, ingredients);

            CollectionAssert.AreEqual(ingredients, recipeCreator.Recipe.Ingredients);
            CollectionAssert.AreEqual(ingredients, recipeCreator2.Recipe.Ingredients);
        }

        [Test]
        public void RecipeCreatorCommit_ShouldCallContextCreateRecipeMethod()
        {
            var recipeCreator = new RecipeCreator(this.Context);
            this.Context.CRCalledTimes = 0;

            recipeCreator.Commit();

            Assert.AreEqual(1, this.Context.CRCalledTimes);
        }

        [Test]
        public void RecipeCreatorChainingMethods_ShouldCorrectlyDoAllMethods()
        {
            var recipeCreator = new RecipeCreator(this.Context);
            var title = "Title";
            var desc = "Description";
            var method = "Method";

            recipeCreator.SetTitle(title).SetDescription(desc).SetMethod(method);

            Assert.AreEqual(title, recipeCreator.Recipe.Title);
            Assert.AreEqual(desc, recipeCreator.Recipe.Description);
            Assert.AreEqual(method, recipeCreator.Recipe.Method);
        }

        // RecipeLoader Tests
        [Test]
        public void RecipeLoaderSetUserRatingLoading_ShouldSetWithUserRating()
        {
            var recipe = Mock.Recipes.SWEET_AND_SPICY_CHICKEN;
            var recipeLoader = new RecipeLoader(this.Context, recipe);

            var loader = recipeLoader.SetUserRatingLoading();

            Assert.AreEqual(true, recipeLoader.WithUserRating);
        }

        [Test]
        public void RecipeLoaderSetSetCommentCountLoading_ShouldSetWithCommentCount()
        {
            var recipe = Mock.Recipes.SWEET_AND_SPICY_CHICKEN;
            var recipeLoader = new RecipeLoader(this.Context, recipe);

            var loader = recipeLoader.SetCommentCountLoading();

            Assert.AreEqual(true, recipeLoader.WithCommentCount);
        }

        [Test]
        public void RecipeLoaderSetCookbookStatusLoading_ShouldSetWithCookbookStatus()
        {
            var recipe = Mock.Recipes.SWEET_AND_SPICY_CHICKEN;
            var recipeLoader = new RecipeLoader(this.Context, recipe);

            var loader = recipeLoader.SetCookbookStatusLoading();

            Assert.AreEqual(true, recipeLoader.WithCookbookStatus);
        }

        [Test]
        public void RecipeLoaderSetMethodLoading_ShouldSetWithMethod()
        {
            var recipe = Mock.Recipes.SWEET_AND_SPICY_CHICKEN;
            var recipeLoader = new RecipeLoader(this.Context, recipe);

            var loader = recipeLoader.SetMethodLoading();

            Assert.AreEqual(true, recipeLoader.WithMethod);
        }

        [Test]
        public void RecipeLoaderSetPermalinkLoading_ShouldSetWithPermalink()
        {
            var recipe = Mock.Recipes.SWEET_AND_SPICY_CHICKEN;
            var recipeLoader = new RecipeLoader(this.Context, recipe);

            var loader = recipeLoader.SetPermalinkLoading();

            Assert.AreEqual(true, recipeLoader.WithPermalink);
        }

        [Test]
        public void RecipeLoaderLoad_ShouldAddARecipeToTheCollection()
        {
            var recipe = Mock.Recipes.SWEET_AND_SPICY_CHICKEN;
            var recipe2 = Mock.Recipes.QUICK_SANDWHICH;
            var recipeLoader = new RecipeLoader(this.Context, recipe);

            var loader = recipeLoader.Load(recipe2);

            CollectionAssert.AreEqual(new List<Recipe>() { recipe, recipe2 }, recipeLoader.RecipesToLoad);
        }

        [Test]
        public void RecipeLoaderList_ShouldCallContextReadRecipe()
        {
            var recipe = Mock.Recipes.SWEET_AND_SPICY_CHICKEN;
            var recipeLoader = new RecipeLoader(this.Context, recipe);
            recipeLoader.SetMethodLoading();
            recipeLoader.SetPermalinkLoading();
            this.Context.RRCalledTimes = 0;

            var loader = recipeLoader.List();

            Assert.AreEqual(1, this.Context.RRCalledTimes);

            Assert.AreEqual(recipeLoader.WithCommentCount, this.Context.options.ReturnCommentCount);
            Assert.AreEqual(recipeLoader.WithCookbookStatus, this.Context.options.ReturnCookbookStatus);
            Assert.AreEqual(recipeLoader.WithUserRating, this.Context.options.ReturnUserRating);
            Assert.AreEqual(recipeLoader.WithPermalink, this.Context.options.ReturnPermalink);
            Assert.AreEqual(recipeLoader.WithMethod, this.Context.options.ReturnMethod);

        }

        // RecipeFinder Tests
        [Test]
        public void RecipeFinderResults_ShouldCallContextRecipeSearch()
        {
            var testQuery = new RecipeQuery();
            var recipeFinder = new RecipeFinder(this.Context, testQuery);

            this.Context.RSCalledTimes = 0;

            var search = recipeFinder.Results();

            var query = this.Context.search;
            Assert.AreEqual(1, this.Context.RSCalledTimes);

            CollectionAssert.AreEqual(query.Include, new Guid[0]);
            CollectionAssert.AreEqual(query.Exclude, new Guid[0]);
            Assert.AreEqual(query.Keywords, null);
            Assert.AreEqual(query.Meal, MealFilter.All);
            Assert.AreEqual(query.Photos, PhotoFilter.All);
            Assert.AreEqual(query.Rating, Rating.None);
            Assert.AreEqual(SortOrder.Rating, query.Sort);
            Assert.AreEqual(SortDirection.Descending, query.Direction);

            Assert.AreEqual(query.Time, new TimeFilter());
            Assert.AreEqual(query.Diet, new DietFilter());
            Assert.AreEqual(query.Nutrition, new NutritionFilter());
            Assert.AreEqual(query.Skill, new SkillFilter());
            Assert.AreEqual(query.Taste, new TasteFilter());
        }
    }
}
