using System;
using KitchenPC.ShoppingLists;

namespace KitchenPC.Ingredients
{
    using System.Web.DynamicData;

    public class IngredientAggregation : IShoppingListSource
    {
        private Ingredient ingredient;

        private Amount amount;

        public IngredientAggregation(Ingredient ingredient)
        {
            this.Ingredient = ingredient;
            this.Amount = new Amount
                              {
                                  Unit = Unit.GetDefaultUnitType(ingredient.ConversionType)
                              };
        }

        public IngredientAggregation(Ingredient ingredient, Amount baseAmount)
        {
            this.Ingredient = ingredient;
            this.Amount = baseAmount;
        }

        public Ingredient Ingredient
        {
            get
            {
                return this.ingredient;
            }

            set
            {
                if (value == null)
                {
                    return;
                }
                this.ingredient = value;
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

        public override string ToString()
        {
            if (Ingredient != null && Amount != null)
            {
                return string.Format("{0}: {1}", Ingredient.Name, Amount);
            }

            return this.Ingredient != null ? this.Ingredient.Name : string.Empty;
        }

        public virtual IngredientAggregation AddUsage(IngredientUsage ingredient)
        {
            if (ingredient.Ingredient.Id != this.Ingredient.Id)
            {
                throw new ArgumentException("Can only call IngredientAggregation::AddUsage() on original ingredient.");
            }

            // Calculate new total
            if (this.Amount.Unit == ingredient.Amount.Unit
                || UnitConverter.CanConvert(this.Amount.Unit, ingredient.Amount.Unit)) //Just add
            {
                this.Amount += ingredient.Amount;
            }
            else
            {
                // Find a conversion path between Ingredient and Form
                var amount = FormConversion.GetNativeAmountForUsage(this.Ingredient, ingredient);
                this.Amount += amount;
            }

            return this; // Allows AddUsage calls to be chained together
        }

        public virtual ShoppingListItem GetItem()
        {
            return new ShoppingListItem(Ingredient) { Amount = Amount };
        }
    }
}