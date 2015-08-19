namespace KitchenPC.Categorization.Models
{
    using KitchenPC.Categorization.Interfaces;
    using KitchenPC.Recipes;
    using KitchenPC.Recipes.Enums;

    public class RecipeClassification : IRecipeClassification
    {
        public RecipeClassification(Recipe recipe, RecipeTag tag)
        {
            this.Recipe = recipe;

            this.IsBreakfast = tag == RecipeTag.Breakfast;
            this.IsLunch = tag == RecipeTag.Lunch;
            this.IsDinner = tag == RecipeTag.Dinner;
            this.IsDessert = tag == RecipeTag.Dessert;
        }

        public Recipe Recipe { get; set; }

        public bool IsBreakfast { get; set; }

        public bool IsLunch { get; set; }

        public bool IsDinner { get; set; }

        public bool IsDessert { get; set; }
    }
}
