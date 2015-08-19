using System;
using System.Collections;
using System.Collections.Generic;

namespace KitchenPC.ShoppingLists
{
    public class ShoppingList : IEnumerable<ShoppingListItem>
    {
        public static Guid GUID_WATER = new Guid("cb44df2d-f27c-442a-bd6e-fd7bdd501f10");

        public Guid? Id { get; set; }

        public string Title { get; set; }

        private readonly List<ShoppingListItem> list;

        private static readonly ShoppingList defaultList = new ShoppingList(null, string.Empty);

        public static ShoppingList DefaultList
        {
            get
            {
                return defaultList;
            }
        }

        public static ShoppingList FromId(Guid menuId)
        {
            return new ShoppingList(menuId, null);
        }

        public ShoppingList()
        {
            this.list = new List<ShoppingListItem>();
        }

        public ShoppingList(Guid? id, String title)
            : this()
        {
            this.Id = id;
            this.Title = title;
        }

        public ShoppingList(Guid? id, String title, IEnumerable<IShoppingListSource> items)
            : this(id, title)
        {
            AddItems(items);
        }

        public void AddItems(IEnumerable<IShoppingListSource> items)
        {
            foreach (var item in items)
            {
                AddItem(item.GetItem());
            }
        }

        private void AddItem(ShoppingListItem item)
        {
            var existingItem = this.list.Find(i => i.Equals(item));
            if (existingItem == null)
            {
                this.list.Add(item);
                return;
            }

            existingItem.CrossedOut = item.CrossedOut; // If new item is crossed out, cross out existing item

            if (existingItem.Ingredient == null || existingItem.Amount == null)
            {
                // Adding same ingredient twice, but nothing to aggregate. Skip.
                return;
            }

            if (item.Amount == null) // Clear out existing amount
            {
                existingItem.Amount = null;
                return;
            }

            //increment existing amount
            existingItem.Amount += item.Amount;
        }

        public override string ToString()
        {
            var title = !string.IsNullOrEmpty(Title) ? Title : "Default List";

            var count = this.list.Count;
            
            return string.Format("{0} ({1} Item{2})", title, count, count != 1 ? "s" : string.Empty);
        }

        public IEnumerator<ShoppingListItem> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }
    }
}