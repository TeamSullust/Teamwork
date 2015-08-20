namespace KitchenPC
{
    using KitchenPC.Ingredients;

    public class InvalidFormException : KPCException
    {
        public InvalidFormException(Ingredient ing, IngredientForm form)
        {
            Ingredient = ing;
            Form = form;
        }

        public Ingredient Ingredient { get; private set; }

        public IngredientForm Form { get; private set; }
    }
}