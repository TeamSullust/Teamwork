using System;

namespace KitchenPC.ShoppingLists
{
    public class ShoppingListResult
    {
        public Guid? NewShoppingListId { get; set; }

        public ShoppingList List { get; set; }
    }
}