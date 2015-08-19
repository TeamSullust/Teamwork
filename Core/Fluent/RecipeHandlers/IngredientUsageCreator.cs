namespace KitchenPC.Fluent.RecipeHandlers
{
    using KitchenPC.Ingredients;

    public class IngredientUsageCreator
    {
        public IngredientUsageCreator(IngredientUsage usage)
        {
            this.Usage = usage;
        }

        public IngredientUsage Usage { get; private set; }

        public IngredientUsageCreator WithIngredient(Ingredient ingredient)
        {
            this.Usage.Ingredient = ingredient;
            return this;
        }

        public IngredientUsageCreator WithForm(IngredientForm form)
        {
            this.Usage.Form = form;
            return this;
        }

        public IngredientUsageCreator WithAmount(Amount amount)
        {
            this.Usage.Amount = amount;
            return this;
        }

        public IngredientUsageCreator WithAmount(float size, Units unit)
        {
            this.Usage.Amount = new Amount(size, unit);
            return this;
        }

        public IngredientUsageCreator WithPrepNote(string prepNote)
        {
            this.Usage.PrepNote = prepNote;
            return this;
        }

        public IngredientUsageCreator InSection(string section)
        {
            this.Usage.Section = section;
            return this;
        }
    }
}
