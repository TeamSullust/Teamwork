namespace KitchenPC.Categorization.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// The interface that is implemented by the DBLoader concrete class
    /// </summary>
    public interface IDBLoader
    {
        /// <summary>
        /// The method creates an array of IIngredientCommonalities from a set of ingredients and sends them back to the caller.
        /// </summary>
        /// <returns>Returns an IEnumerable of IIngredientCommonalities.</returns>
        IEnumerable<IIngredientCommonality> LoadCommonIngredients();

        /// <summary>
        /// The method creates an array of IRecipeClassifications as training data and returns it to the caller.
        /// </summary>
        /// <returns>Returns and IEnumerable of IRecipeClassifications.</returns>
        IEnumerable<IRecipeClassification> LoadTrainingData();
    }
}