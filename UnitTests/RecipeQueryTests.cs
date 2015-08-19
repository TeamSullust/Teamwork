namespace KitchenPC.UnitTests
{
    using System;

    using KitchenPC.Recipes;
    using KitchenPC.Recipes.Enums;
    using KitchenPC.Recipes.Filters;

    using NUnit.Framework;

    [TestFixture]
    public class RecipeQueryTests
    {
        [Test]
        public void TestRecipeQueryConstructor()
        {
            var query = new RecipeQuery();

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

        [Test]
        public void RecipeQueryClone_ShouldCloneObject()
        {
            var query = new RecipeQuery
                            {
                                Keywords = "clone",
                                Meal = MealFilter.Breakfast,
                                Rating = Rating.OneStar,
                                Sort = SortOrder.CookTime,
                                Direction = SortDirection.Ascending,
                                Offset = 3,
                                Photos = PhotoFilter.HighRes,
                                Taste =
                                    {
                                        MildToSpicy = SpicinessLevel.Spicy,
                                        SavoryToSweet = SweetnessLevel.Sweet
                                    },
                                Include = new Guid[] { Guid.NewGuid(), },
                                Exclude = new Guid[] { Guid.NewGuid(), Guid.NewGuid() }
                            };

            var clonedQuery = query.Clone() as RecipeQuery;

            Assert.AreEqual(query.Keywords, clonedQuery.Keywords);
            Assert.AreEqual(query.Meal, clonedQuery.Meal);
            Assert.AreEqual(query.Rating, clonedQuery.Rating);
            Assert.AreEqual(query.Sort, clonedQuery.Sort);
            Assert.AreEqual(query.Direction, clonedQuery.Direction);
            Assert.AreEqual(query.Offset, clonedQuery.Offset);
            Assert.AreEqual(query.Photos, clonedQuery.Photos);

            Assert.AreEqual(query.Time, clonedQuery.Time);
            Assert.AreNotSame(query.Time, clonedQuery.Time);

            Assert.AreEqual(query.Diet, clonedQuery.Diet);
            Assert.AreNotSame(query.Diet, clonedQuery.Diet);

            Assert.AreEqual(query.Nutrition, clonedQuery.Nutrition);
            Assert.AreNotSame(query.Nutrition, clonedQuery.Nutrition);

            Assert.AreEqual(query.Skill, clonedQuery.Skill);
            Assert.AreNotSame(query.Skill, clonedQuery.Skill);

            Assert.AreEqual(query.Taste, clonedQuery.Taste);
            Assert.AreNotSame(query.Taste, clonedQuery.Taste);

            CollectionAssert.AreEqual(query.Include, clonedQuery.Include);
            Assert.AreNotSame(query.Include, clonedQuery.Include);

            CollectionAssert.AreEqual(query.Exclude, clonedQuery.Exclude);
            Assert.AreNotSame(query.Exclude, clonedQuery.Exclude);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RecipeQuerySetInclude_WithANullObject_ShouldThrowArgumentNullException()
        {
            var query = new RecipeQuery { Include = null };

        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RecipeQuerySetExclude_WithANullObject_ShouldThrowArgumentNullException()
        {
            var query = new RecipeQuery { Exclude = null };

        }
    }
}
