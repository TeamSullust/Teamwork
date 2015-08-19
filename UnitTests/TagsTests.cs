namespace KitchenPC.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KitchenPC.Recipes;
    using KitchenPC.Recipes.Enums;
    using NUnit.Framework;

    [TestFixture]
    public class TagsTest
    {
        [Test]
        public void RecipeTagsConstructor_WithASingleParameter_ShouldConstructACorrectRecipeTagsObject()
        {
            var tags = new RecipeTags(RecipeTag.LowCalorie);

            CollectionAssert.AreEqual(tags, new RecipeTag[] { RecipeTag.LowCalorie });
        }

        [Test]
        public void RecipeTagsConstructor_WithMultipleParameters_ShouldConstructACorrectRecipeTagsObject()
        {
            var tags = new RecipeTags(RecipeTag.GlutenFree, RecipeTag.NoAnimals, RecipeTag.NoMeat, RecipeTag.NoPork);

            CollectionAssert.AreEqual(tags, new RecipeTag[] { RecipeTag.GlutenFree, RecipeTag.NoAnimals, RecipeTag.NoMeat, RecipeTag.NoPork });
        }

        [Test]
        public void RecipeTagsLength_ShouldReturnCorrectLengthOfTheCollection()
        {
            var tags1 = new RecipeTags(RecipeTag.GlutenFree, RecipeTag.NoAnimals, RecipeTag.NoMeat, RecipeTag.NoPork);
            var tags2 = new RecipeTags(RecipeTag.Lunch, RecipeTag.GlutenFree);

            // Test counts
            Assert.AreEqual(4, tags1.Length);
            Assert.AreEqual(2, tags2.Length);
        }

        [Test]
        public void RecipeTags_ComparisonOperatorsShouldReturnCorrectResult()
        {
            var tags1 = new RecipeTags(RecipeTag.GlutenFree, RecipeTag.NoAnimals, RecipeTag.NoMeat, RecipeTag.NoPork);
            var tags2 = new RecipeTags(RecipeTag.Lunch, RecipeTag.GlutenFree);
            var tags3 = tags1;
            var tags4 = new RecipeTags(RecipeTag.Lunch, RecipeTag.GlutenFree);

            //Test comparison operators
            Assert.IsTrue(tags1 == tags3);

            Assert.IsFalse(tags1 == tags2);
            Assert.IsFalse(tags2 == tags4);

            Assert.IsFalse(tags1 != tags3);

            Assert.IsTrue(tags1 != tags2);
            Assert.IsTrue(tags2 != tags4);
        }

        [Test]
        public void RecipeTags_WithCorrectIndex_ShouldReturnCorrectElement()
        {
            var tags1 = new RecipeTags(RecipeTag.GlutenFree, RecipeTag.NoAnimals, RecipeTag.NoMeat, RecipeTag.NoPork);

            // Test indexing
            Assert.IsTrue(tags1[0] == RecipeTag.GlutenFree);
            Assert.IsTrue(tags1[2] == RecipeTag.NoMeat);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void RecipeTags_WithIncorrectIndex_ShouldThrowAnIndexOutOfRangeException()
        {
            var tags1 = new RecipeTags(RecipeTag.GlutenFree, RecipeTag.NoAnimals, RecipeTag.NoMeat, RecipeTag.NoPork);

            var tag = tags1[-1];
        }

        [Test]
        public void RecipeTagsAddMethod_ShouldIncreseLengthPropertyCorrectly()
        {
            var tags1 = new RecipeTags();
            Assert.AreEqual(tags1.Length, 0);

            tags1.Add(RecipeTag.Breakfast);
            tags1.Add(RecipeTag.Dessert);

            Assert.AreEqual(2, tags1.Length);
        }

        [Test]
        public void RecipeTagsAddMethod_ShouldAddACorrectElementToTheCollection()
        {
            var tags1 = new RecipeTags();

            tags1.Add(RecipeTag.Breakfast);
            tags1.Add(RecipeTag.Dessert);

            Assert.AreEqual(tags1[0], RecipeTag.Breakfast);
            Assert.AreEqual(tags1[1], RecipeTag.Dessert);
        }

        [Test]
        public void RecipeTagsHasTag_ShouldReturnCorrectResult()
        {
            var tags1 = new RecipeTags(RecipeTag.EasyToMake, RecipeTag.CommonIngredients);

            Assert.IsTrue(tags1.Contains(RecipeTag.EasyToMake));
            Assert.IsTrue(tags1.Contains(RecipeTag.CommonIngredients));
            Assert.IsFalse(tags1.Contains(RecipeTag.Breakfast));
            Assert.IsFalse(tags1.Contains(RecipeTag.LowCalorie));
        }

        [Test]
        public void RecipeTagsIterator_ShouldIterateTheCollectionCorrectly()
        {
            // Test Iteration
            var tags = new List<RecipeTag>();
            var tags2 = new RecipeTags(RecipeTag.Breakfast, RecipeTag.Dessert, RecipeTag.LowCalorie);
            tags.AddRange(tags2);

            Assert.AreEqual(3, tags.Count);
            Assert.AreEqual(tags[0], RecipeTag.Breakfast);
            Assert.AreEqual(tags[1], RecipeTag.Dessert);
            Assert.AreEqual(tags[2], RecipeTag.LowCalorie);
        }

        [Test]
        public void RecipeTagsToString_ShouldReturnCorrectValue()
        {
            // Test ToString
            var tags = new RecipeTags(RecipeTag.GlutenFree, RecipeTag.NoAnimals, RecipeTag.NoMeat, RecipeTag.NoPork);

            Assert.AreEqual("GlutenFree, NoAnimals, NoMeat, NoPork", tags.ToString());
        }

        [Test]
        public void TestParsing()
        {
            var tags = RecipeTags.Parse("No Red Meat, Dinner, Lunch");

            Assert.AreEqual(3, tags.Length);
            Assert.AreEqual("NoRedMeat, Dinner, Lunch", tags.ToString());
        }
    }
}