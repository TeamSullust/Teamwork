namespace KitchenPC.Menus
{
    public class GetMenuOptions
    {
        public bool hasLoadedRecipes;

        private static readonly GetMenuOptions none = new GetMenuOptions();

        private static readonly GetMenuOptions Loaded = new GetMenuOptions
                                                            {
                                                                hasLoadedRecipes = true
                                                            };

        public static GetMenuOptions None
        {
            get
            {
                return none;
            }
        }

        public static GetMenuOptions WithRecipes
        {
            get
            {
                return Loaded;
            }
        }
    }
}