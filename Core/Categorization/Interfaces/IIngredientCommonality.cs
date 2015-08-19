namespace KitchenPC.Categorization.Interfaces
{
    using System;

    /// <summary>
    /// The interface that is implemented by the IngredientCommonality class.
    /// </summary>
    public interface IIngredientCommonality
    {
        /// <summary>
        /// An unique global id Identifier carrying the ingredient ID.
        /// </summary>
        Guid IngredientId { get; }

        /// <summary>
        /// A float number representing how common the ingredient is.
        /// </summary>
        float Commonality { get; }
    }
}