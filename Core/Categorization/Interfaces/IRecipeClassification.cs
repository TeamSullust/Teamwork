namespace KitchenPC.Categorization.Interfaces
{
    using KitchenPC.Recipes;

    /// <summary>
    /// The interface that is implemented by the RecipeClassification class.
    /// </summary>
    public interface IRecipeClassification
    {
        /// <summary>
        /// A property that holds the Recipe that the RecipeClassification classifies.
        /// </summary>
        Recipe Recipe { get; }

        /// <summary>
        /// A bool property that classiefies if the Recipe is a Breakfast recipe or not.
        /// </summary>
        bool IsBreakfast { get; }

        /// <summary>
        /// A bool property that classiefies if the Recipe is a Lunch recipe or not.
        /// </summary>
        bool IsLunch { get; }

        /// <summary>
        /// A bool property that classiefies if the Recipe is a Dinner recipe or not.
        /// </summary>
        bool IsDinner { get; }

        /// <summary>
        /// A bool property that classiefies if the Recipe is a Dessert recipe or not.
        /// </summary>
        bool IsDessert { get; }
    }
}