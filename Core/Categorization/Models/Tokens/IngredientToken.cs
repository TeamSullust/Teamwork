namespace KitchenPC.Categorization.Models.Tokens
{
    using KitchenPC.Categorization.Interfaces;
    using KitchenPC.Ingredients;

    public class IngredientToken : IToken
    {
        public IngredientToken(Ingredient ing)
        {
            this.Ingredient = ing;
        }

        public Ingredient Ingredient { get; private set; }

        public override bool Equals(object obj)
        {
            var t1 = obj as IngredientToken;
            return t1 != null && t1.Ingredient.Id.Equals(this.Ingredient.Id);
        }

        public override int GetHashCode()
        {
            return this.Ingredient.Id.GetHashCode();
        }

        public override string ToString()
        {
            return "[ING] - " + this.Ingredient;
        }
    }
}