namespace KitchenPC.Categorization.Logic
{
    using System;
    using System.Collections.Generic;

    using KitchenPC.Categorization.Interfaces;
    using KitchenPC.Categorization.Models;
    using KitchenPC.Ingredients;
    using KitchenPC.Recipes;
    using KitchenPC.Recipes.Enums;

    public class CategorizationDBLoader : IDBLoader
    {
        private static readonly Ingredient[] CommonIngredients =
            {
                new Ingredient(
                    new Guid("a6b0179f-6bf5-4ec4-b12b-a95f2f94fe91"),
                    "salt",
                    new IngredientMetadata(
                    false,
                    false,
                    false,
                    false,
                    false,
                    0,
                    0,
                    0f,
                    0f,
                    0f,
                    38758f,
                    0f)),
                new Ingredient(
                    new Guid("4de12110-87b1-4198-a9a9-2b32e45df0f0"),
                    "granulated sugar",
                    new IngredientMetadata(
                    false,
                    false,
                    false,
                    false,
                    true,
                    0,
                    4,
                    0f,
                    99.8f,
                    387f,
                    1f,
                    99.98f)),
                new Ingredient(
                    new Guid("948aeda5-ffff-41bd-af4e-71d1c740db76"),
                    "eggs",
                    new IngredientMetadata(
                    false,
                    false,
                    false,
                    false,
                    true,
                    0,
                    0,
                    9.51f,
                    0.37f,
                    143f,
                    142f,
                    0.72f)),
                new Ingredient(
                    new Guid("daa8fbf6-3347-41b9-826d-078cd321402e"),
                    "all-purpose flour",
                    new IngredientMetadata(
                    true,
                    false,
                    false,
                    false,
                    false,
                    0,
                    0,
                    0.98f,
                    0.27f,
                    364f,
                    2f,
                    76.31f)),
                new Ingredient(
                    new Guid("ac48a403-b5d7-42f1-bafb-aed0c597f139"),
                    "salted butter",
                    new IngredientMetadata(
                    false,
                    false,
                    false,
                    false,
                    true,
                    0,
                    0,
                    81.11f,
                    0.06f,
                    717f,
                    714f,
                    0.06f)),
                new Ingredient(
                    new Guid("6a51bee2-8a40-4c7e-bf91-c7a29e980ac6"),
                    "vanilla extract",
                    new IngredientMetadata(
                    false,
                    false,
                    false,
                    false,
                    false,
                    0,
                    0,
                    0.06f,
                    12.65f,
                    288f,
                    9f,
                    12.65f)),
                new Ingredient(
                    new Guid("cb44df2d-f27c-442a-bd6e-fd7bdd501f10"),
                    "water",
                    new IngredientMetadata(
                    false,
                    false,
                    false,
                    false,
                    false,
                    0,
                    0,
                    0f,
                    0f,
                    0f,
                    4f,
                    0f)),
                new Ingredient(
                    new Guid("514c3e35-1c23-44ed-9438-7f41244b852f"),
                    "black pepper",
                    new IngredientMetadata(
                    false,
                    false,
                    false,
                    false,
                    false,
                    0,
                    0,
                    3.26f,
                    0.64f,
                    251f,
                    20f,
                    63.95f)),
                new Ingredient(
                    new Guid("ab207e00-907c-44d9-8afe-a8931b899b0c"),
                    "baking powder",
                    new IngredientMetadata(
                    false,
                    false,
                    false,
                    false,
                    false,
                    0,
                    0,
                    0f,
                    0f,
                    53f,
                    10600f,
                    27.7f)),
                new Ingredient(
                    new Guid("5a698842-54a9-4ed2-b6c3-aea1bcd157cd"),
                    "2% milk",
                    new IngredientMetadata(
                    false,
                    false,
                    false,
                    false,
                    true,
                    0,
                    0,
                    1.98f,
                    5.06f,
                    50f,
                    47f,
                    4.8f)),
                new Ingredient(
                    new Guid("2d1a807c-5102-4ef4-9620-5e951c8365f9"),
                    "light brown sugar",
                    new IngredientMetadata(
                    false,
                    false,
                    false,
                    false,
                    true,
                    0,
                    4,
                    0f,
                    97.02f,
                    380f,
                    28f,
                    98.09f)),
                new Ingredient(
                    new Guid("63e799e8-8c12-4302-931e-f058923d97d1"),
                    "baking soda",
                    new IngredientMetadata(
                    false,
                    false,
                    false,
                    false,
                    false,
                    0,
                    0,
                    0f,
                    0f,
                    0f,
                    27360f,
                    0f)),
                new Ingredient(
                    new Guid("bef2bde5-d6b3-45d9-95e9-eb75e05721d4"),
                    "unsweetened baking chocolate squares",
                    new IngredientMetadata(
                    false,
                    false,
                    false,
                    false,
                    true,
                    0,
                    1,
                    52.31f,
                    0.91f,
                    501f,
                    24f,
                    29.84f)),
                new Ingredient(
                    new Guid("5b42f415-8012-48b6-86e6-576a2f1dac83"),
                    "margarine (oleo)",
                    new IngredientMetadata(
                    false,
                    false,
                    false,
                    false,
                    true,
                    0,
                    0,
                    60.39f,
                    0f,
                    537f,
                    785f,
                    0.69f)),
            };

        public IEnumerable<IIngredientCommonality> LoadCommonIngredients()
        {
            return new IngredientCommonality[]
                       {
                           new IngredientCommonality(CommonIngredients[0].Id, 1f),
                           new IngredientCommonality(CommonIngredients[1].Id, 0.90f),
                           new IngredientCommonality(CommonIngredients[2].Id, 0.88f),
                           new IngredientCommonality(CommonIngredients[3].Id, 0.76f),
                           new IngredientCommonality(CommonIngredients[4].Id, 0.63f),
                           new IngredientCommonality(CommonIngredients[5].Id, 0.50f),
                           new IngredientCommonality(CommonIngredients[6].Id, 0.40f),
                           new IngredientCommonality(CommonIngredients[7].Id, 0.35f),
                           new IngredientCommonality(CommonIngredients[8].Id, 0.35f),
                           new IngredientCommonality(CommonIngredients[9].Id, 0.33f),
                           new IngredientCommonality(CommonIngredients[10].Id, 0.32f),
                           new IngredientCommonality(CommonIngredients[11].Id, 0.31f)
                       };
        }

        public IEnumerable<IRecipeClassification> LoadTrainingData()
        {
            // Some training data to classify other recipes
            return new RecipeClassification[]
                       {
                           // Breakfast
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Patriotic French Toast",
                               Description = "French toast with a cream cheese topping and fresh fruit!",
                               },
                               RecipeTag.Breakfast),
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Blueberry and Raspberry Pancake Topping",
                               Description = "Blueberries and raspberries mingle in this thick, rich, delicious topping for pancakes or waffles.",
                               },
                               RecipeTag.Breakfast),
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Basic Crepes",
                               Description = "Here is a simple but delicious crepe batter which can be made in minutes. It's made  from ingredients that everyone has on hand.",
                               },
                               RecipeTag.Breakfast),
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Health Nut Blueberry Muffins",
                               Description = "An awesome healthy alternative to the usual blueberry muffin.",
                               },
                               RecipeTag.Breakfast),
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Peanut Butter Banana Smoothie",
                               Description = "It is so refreshing and it's sweet and tasty.",
                               },
                               RecipeTag.Breakfast),

                           // Lunch
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Turkey Wild Rice Soup",
                               Description = "a hearty soup",
                               },
                               RecipeTag.Lunch),
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Elise's Favorite Waldorf Salad",
                               Description = "A slightly sweet salad with a touch of cinnamon.",
                               },
                               RecipeTag.Lunch),
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Wonderful Chicken Curry Salad",
                               Description = "I created this salad for a party last year.  Serve on croissants or lettuce leaves. For a fancy presentation, line a platter with red leaf lettuce, and top with cream puff shells that have been stuffed.",
                               },
                               RecipeTag.Lunch),
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Cream Cheese and Ham Spread",
                               Description = "Simple, but flavorful, spread that is great with crackers or celery.",
                               },
                               RecipeTag.Lunch),
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Cucumber Dip II",
                               Description = "A cool, fresh-tasting dip, perfect with pretzels, vegetables, and chips.",
                               },
                               RecipeTag.Lunch),

                           // Dinner
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Homemade Italian Turkey Sausage",
                               Description = "Pens Joyce Haworth from Des Plaines, Illinois, 'When the stores in my area stopped carrying our favorite turkey sausage, I was desperate! I went to the library for some books on sausage-making...and was surprised to learn how easy it is! We use this sweet",
                               },
                               RecipeTag.Dinner),
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Barbeque Pork Two Ways",
                               Description = "Easy and delicious! Pork shoulder, slow-cooked or simmered on the stovetop with onion and spices. Serve hot in sandwich buns.",
                               },
                               RecipeTag.Dinner),
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Polynesian Ribs",
                               Description = "A friend shared this recipe more than 30 years ago, and I've been using it ever since. I make the ribs a day ahead and let the flavors meld, then I reheat and serve them the next day.",
                               },
                               RecipeTag.Dinner),
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Parmesan Crusted Dinner Rolls",
                               Description = "These are delicious white bread rolls that are dipped in butter, then rolled in Parmesan and left to rise. These are especially great for Thanksgiving and Christmas.",
                               },
                               RecipeTag.Dinner),
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Glazed Meatloaf II",
                               Description = "This meatloaf is great! It's my husband's favorite. The glaze makes it delicious and moist.",
                               },
                               RecipeTag.Dinner),

                           // Desserts
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Banana Oatmeal Cookies",
                               Description = "Spicy oatmeal cookies with banana and walnuts",
                               },
                               RecipeTag.Dessert),
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Cheesecake",
                               Description = "I chose to combine the cheesecake with a devils food cake, creating three layers with a rich butter-cream icing between each layer. Then I covered it with more icing and chocolate chips.",
                               },
                               RecipeTag.Dessert),
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Chocolate Toffee Bars",
                               Description = "Tastes like English Toffee ",
                               },
                               RecipeTag.Dessert),
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Eclair Cake",
                               Description = "No bake -- Super easy!",
                               },
                               RecipeTag.Dessert),
                           new RecipeClassification(
                               new Recipe
                               {
                               Id = Guid.NewGuid(),
                               Title = "Rum Cakes",
                               Description = "Mini Bundts -- Adorable!",
                               },
                               RecipeTag.Dessert)
                       };
        }
    }
}
