namespace KitchenPC
{
    public class InvalidRecipeDataException : KPCException
    {
        public InvalidRecipeDataException(string errorMessage)
            : base(errorMessage)
        {
        }
    }
}