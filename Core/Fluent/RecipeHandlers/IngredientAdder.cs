namespace KitchenPC.Fluent.RecipeHandlers
{
    using KitchenPC.Context;
    using KitchenPC.Ingredients;
    using KitchenPC.NLP;
    using KitchenPC.Recipes;

    public class IngredientAdder
    {
        private readonly IKPCContext context;

        private readonly Recipe recipe;

        private readonly string section;

        public IngredientAdder(IKPCContext context, Recipe recipe)
        {
            this.context = context;
            this.recipe = recipe;
        }

        public IngredientAdder(IKPCContext context, Recipe recipe, string section)
            : this(context, recipe)
        {
            this.section = section;
        }

        public IngredientAdder AddIngredientUsage(IngredientUsage usage)
        {
            if (!string.IsNullOrWhiteSpace(this.section))
            {
                usage.Section = this.section;
            }

            this.recipe.AddIngredient(usage);
            return this;
        }

        public IngredientAdder AddIngredient(Ingredient ingredient, Amount amount, string prepNote = null)
        {
            var usage = new IngredientUsage(ingredient, null, amount, prepNote);
            return this.AddIngredientUsage(usage);
        }

        public IngredientAdder AddIngredient(Ingredient ingredient, string prepNote = null)
        {
            return this.AddIngredient(ingredient, null, prepNote);
        }

        public IngredientAdder AddRaw(string raw)
        {
            var result = this.context.ParseIngredientUsage(raw);

            if (result is Match)
            {
                return this.AddIngredientUsage(result.Usage);
            }

            throw new CouldNotParseUsageException(result, raw);
        }
    }
}
