namespace KitchenPC.ShoppingLists
{
    public class GetShoppingListOptions
    {
        public bool HasLoadedItems;

        private static readonly GetShoppingListOptions none = new GetShoppingListOptions();

        private static readonly GetShoppingListOptions Loaded = new GetShoppingListOptions
                                                                    {
                                                                        HasLoadedItems = true
                                                                    };

        public static GetShoppingListOptions None
        {
            get
            {
                return none;
            }
        }

        public static GetShoppingListOptions WithItems
        {
            get
            {
                return Loaded;
            }
        }
    }
}