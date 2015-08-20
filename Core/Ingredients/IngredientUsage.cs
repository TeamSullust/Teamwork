using System;
using KitchenPC.Context.Fluent;

namespace KitchenPC.Ingredients
{
    using KitchenPC.Fluent.RecipeHandlers;

    public class IngredientUsage
    {
        private Ingredient ingredient;

        private IngredientForm form;

        private Amount amount;

        private string prepNote;

        private string section;

        public static IngredientUsageCreator Create
        {
            get
            {
                return new IngredientUsageCreator(new IngredientUsage());
            }
        }

        public IngredientUsage(Ingredient ingredient, IngredientForm form, Amount amount, string prepnote)
        {
            Ingredient = ingredient;
            Form = form;
            Amount = amount;
            PrepNote = prepnote;
        }

        public IngredientUsage()
        {
        }

        public Ingredient Ingredient
        {
            get
            {
                return this.ingredient;
            }

            set
            {
                this.ingredient = value;
            }
        }

        public IngredientForm Form
        {
            get
            {
                return this.form;
            }

            set
            {
                this.form = value;
            }
        }

        public Amount Amount
        {
            get
            {
                return this.amount;
            }

            set
            {
                this.amount = value;
            }
        }

        public string PrepNote
        {
            get
            {
                return this.prepNote;
            }

            set
            {
                this.prepNote = value;
            }
        }

        public string Section
        {
            get
            {
                return this.section;
            }

            set
            {
                this.section = value;
            }
        }

        /// <summary>Renders Ingredient Usage, using ingredientTemplate for the ingredient name.</summary>
        /// <param name="ingredientTemplate">A string template for the ingredient name, {0} will be the Ingredient Id and {1} will be the ingredient name.</param>
        /// <param name="amountTemplate">Optional string template for displaying amounts.  {0} will be numeric value, {1} will be unit.</param>
        /// <param name="multiplier">Number to multiply amount by, used to adjust recipe servings.</param>
        /// <returns>Ingredient Name (form): Amount (prep note)</returns>
        public string ToString(string ingredientTemplate, string amountTemplate, float multiplier)
        {
            var ingredientName = string.IsNullOrEmpty(ingredientTemplate)
                              ? Ingredient.Name
                              : string.Format(ingredientTemplate, Ingredient.Id, Ingredient.Name);
            var prep = string.Empty;
            string amount;

            if (!string.IsNullOrEmpty(PrepNote))
            {
                prep = string.Format(" ({0})", PrepNote);
            }

            if (Amount == null)
            {
                //Just display ingredient and prep
                return string.Format("{0}{1}", ingredientName, prep);
            }

            //Normalize amount and form
            var normalizedAmount = Amount == null ? null : Amount.Normalize(Amount, multiplier);
            if (Form.FormUnitType != Units.Unit && !string.IsNullOrEmpty(Form.FormDisplayName))
            {
                ingredientName += string.Format(" ({0})", Form.FormDisplayName);
            }

            var unitType = Unit.GetConvType(Form.FormUnitType);

            if (unitType == UnitType.Unit && !string.IsNullOrEmpty(Form.FormUnitName))
            {
                var names = Form.FormUnitName.Split('/');
                var unitName = (normalizedAmount.SizeLow.HasValue || normalizedAmount.SizeHigh > 1) ? names[1] : names[0];
                amount = normalizedAmount.ToString(unitName);
            }
            else
            {
                amount = normalizedAmount.ToString();
            }

            var resultAmount = string.Format(string.IsNullOrEmpty(amountTemplate) ? "{0}{1}" : amountTemplate, amount, prep);
            return string.Format("{0}: {1}", ingredientName, resultAmount);
        }

        public string ToString(float multiplier)
        {
            return ToString(null, null, multiplier);
        }

        public override string ToString()
        {
            return ToString(null, null, 1);
        }
    }
}