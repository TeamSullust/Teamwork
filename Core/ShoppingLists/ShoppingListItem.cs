using System;
using KitchenPC.Ingredients;
using KitchenPC.Recipes;

namespace KitchenPC.ShoppingLists
{
    public class ShoppingListItem : IngredientAggregation
    {
        public string Raw { get; set; }

        public Guid? Id { get; set; }

        public RecipeBrief Recipe { get; set; }

        public bool CrossedOut { get; set; }

        public static ShoppingListItem FromId(Guid id)
        {
            return new ShoppingListItem(id);
        }

        public ShoppingListItem(Guid id)
            : base(null)
        {
            this.Id = id;
        }

        public ShoppingListItem(string raw)
            : base(null)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                throw new ArgumentException("Shopping list item cannot be blank.");
            }

            Raw = raw;
        }

        public ShoppingListItem(Ingredient ingredient)
            : base(ingredient)
        {
        }

        public override IngredientAggregation AddUsage(IngredientUsage usage)
        {
            if (Ingredient == null)
            {
                throw new ArgumentException(
                    "Cannot add usage to a non-resolved shopping list item. Create a new shopping list based on an IngredientUsage.");
            }

            return base.AddUsage(usage);
        }

        public override ShoppingListItem GetItem()
        {
            return this;
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(Raw) ? Raw : base.ToString();
        }

        public static implicit operator ShoppingListItem(string item)
        {
            return new ShoppingListItem(item);
        }

        public static implicit operator String(ShoppingListItem itemToString)
        {
            return itemToString.ToString();
        }

        public override bool Equals(object obj)
        {
            var item = obj as ShoppingListItem;

            if (item == null)
            {
                return false;
            }

            if (this.Ingredient != null && item.Ingredient != null)
            {
                // If they both represent an ingredient, compare by ingredient
                return this.Ingredient.Equals(item.Ingredient);
            }

            // Compare by Raw string
            return string.Equals(this.Raw, item.Raw, StringComparison.InvariantCulture);
        }

        public override int GetHashCode()
        {
            return this.Ingredient != null ? this.Ingredient.Id.GetHashCode() : Raw.GetHashCode();
        }
    }
}