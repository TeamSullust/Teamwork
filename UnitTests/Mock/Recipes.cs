using System;
using KitchenPC.Ingredients;
using KitchenPC.Recipes;

namespace KitchenPC.UnitTests.Mock
{
    using KitchenPC.Recipes.Enums;

    internal static class Recipes
    {
        public static Recipe MockRecipe(string title, string desc)
        {
            var ret = new Recipe
                          {
                              Id = Guid.NewGuid(),
                              Title = title,
                              Description = desc,
                              Tags = new RecipeTags(RecipeTag.Breakfast)
                          };

            ret.Method = "This is a mock recipe.";
            ret.OwnerAlias = "Fake Owner";
            ret.OwnerId = Guid.NewGuid();
            ret.Permalink = "http://www.kitchenpc.com/123";
            ret.ServingSize = 5;
            var ingredient = new IngredientUsage { Ingredient = Mock.Ingredients.SALT };
            ret.Ingredients = new IngredientUsage[1] { ingredient };

            return ret;
        }

        public static Recipe BEST_BROWNIES
        {
            get
            {
                var r = new Recipe { Id = new Guid("b11a64a9-95b3-402f-8b82-312bad539d4e"), Title = "Best Brownies", Description = "from scratch!" };

                r.Tags = new RecipeTags(RecipeTag.NoMeat, RecipeTag.NoPork, RecipeTag.NoRedMeat, RecipeTag.Dessert);
                r.AvgRating = 5;
                r.CookTime = 40;
                r.PrepTime = 15;
                r.ServingSize = 24;
                r.Ingredients = new IngredientUsage[]
            {
               new IngredientUsage(Ingredients.MARGARINE, Forms.MARGARINE_VOLUME, new Amount(1, Units.Cup), "in chunks"),
               new IngredientUsage(Ingredients.UNSWEETENED_BAKING_CHOCOLATE_SQUARES, Forms.UNSWEETENED_BAKING_CHOCOLATE_SQUARES_WEIGHT, new Amount(1, Units.Ounce), ""),
               new IngredientUsage(Ingredients.GRANULATED_SUGAR, Forms.GRANULATED_SUGAR_VOLUME, new Amount(2.66667f, Units.Cup), ""),
               new IngredientUsage(Ingredients.EGGS, Forms.EGGS_UNIT, new Amount(4, Units.Unit), "large"),
               new IngredientUsage(Ingredients.VANILLA_EXTRACT, Forms.VANILLA_EXTRACT_VOLUME, new Amount(2, Units.Teaspoon), ""),
               new IngredientUsage(Ingredients.ALL_PURPOSE_FLOUR, Forms.ALL_PURPOSE_FLOUR_SIFTED, new Amount(1, Units.Cup), "")
            };

                return r;
            }
        }

        public static Recipe QUICK_SANDWHICH
        {
            get
            {
                var r = new Recipe { Id = Guid.NewGuid(), Title = "Easy Sandwhich", Description = "Mmm Tasty!" };

                r.Tags = new RecipeTags(RecipeTag.Breakfast);
                r.AvgRating = 5;
                r.CookTime = 20;
                r.PrepTime = 10;
                r.ServingSize = 1;
                r.Ingredients = new IngredientUsage[]
            {
               
               new IngredientUsage(Ingredients.EGGS, Forms.EGGS_UNIT, new Amount(4, Units.Unit), "large"),
               new IngredientUsage(Ingredients.SALTED_BUTTER, Forms.SALTED_BUTTER_VOLUME, new Amount(2, Units.Tablespoon), ""),
               new IngredientUsage(Ingredients.ALL_PURPOSE_FLOUR, Forms.ALL_PURPOSE_FLOUR_SIFTED, new Amount(1, Units.Cup), "")
            };

                return r;
            }
        }

        public static Recipe MIRACLE_DIET
        {
            get
            {
                var r = new Recipe { Id = Guid.NewGuid(), Title = "Miracle Diet", Description = "Literally air" };

                r.Tags = new RecipeTags(RecipeTag.Breakfast);
                r.AvgRating = 5;
                r.CookTime = 20;
                r.PrepTime = 10;
                r.ServingSize = 1;
                var miracleIngredient = new Ingredient(Guid.NewGuid(), "air", new IngredientMetadata(false, false, false, false, false, 0, 0, 0f, 0f, 0f, 0f, 0f));
                r.Ingredients = new IngredientUsage[]
            {
               
               new IngredientUsage(miracleIngredient, Forms.SALT_WEIGHT, new Amount(50, Units.Gram), ""),
            };

                return r;
            }
        }

        public static Recipe SWEET_AND_SPICY_CHICKEN
        {
            get
            {
                var r = new Recipe { Id = Guid.NewGuid(), Title = "Sweet and Spicy Chicken", Description = "Now with 500% less chicken!" };

                r.Tags = new RecipeTags(RecipeTag.Breakfast);
                r.AvgRating = 5;
                r.CookTime = 20;
                r.PrepTime = 10;
                r.ServingSize = 1;
                var sweetsAndSpices = new Ingredient(Guid.NewGuid(), "sweets and spices", new IngredientMetadata(false, false, false, false, false, 5, 5, 0f, 0f, 0f, 0f, 0f));
                r.Ingredients = new IngredientUsage[]
            {
               
               new IngredientUsage(sweetsAndSpices, Forms.SALT_WEIGHT, new Amount(50, Units.Gram), ""),
            };

                return r;
            }
        }
    }
}