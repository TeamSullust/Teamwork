using System;
using KitchenPC.Recipes;

namespace KitchenPC.Menus
{
    public struct Menu
    {
        private Guid? id;

        private String title;

        private RecipeBrief[] recipes; //Can be null

        public static Menu FromId(Guid menuId)
        {
            return new Menu(menuId, null);
        }

        private static readonly Menu favorites = new Menu(null, "Favorites");

        public static Menu Favorites
        {
            get
            {
                return favorites;
            }
        }

        public Menu(Guid? id, string title)
        {
            this.id = id;
            this.title = title;
            recipes = null;
        }

        public Menu(Menu menu)
        {
            id = menu.Id;
            title = menu.Title;
            recipes = null;

            if (menu.Recipes == null)
            {
                return;
            }
            this.Recipes = new RecipeBrief[menu.Recipes.Length];
            menu.Recipes.CopyTo(this.Recipes, 0);
        }

        public Guid? Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        public String Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        public RecipeBrief[] Recipes
        {
            get
            {
                return this.recipes;
            }
            set
            {
                this.recipes = value;
            }
        }

        public override string ToString()
        {
            var count = (Recipes != null ? Recipes.Length : 0);
            var result = string.Format("{0} ({1} {2}", Title, count, count != 1 ? "recipes" : "recipe");
            return result;
        }

        public override bool Equals(object obj)
        {
            if (false == (obj is Menu))
            {
                return false;
            }

            var menu = (Menu)obj;
            if (this.Id.HasValue || menu.Id.HasValue)
            {
                return this.Id.Equals(menu.Id);
            }

            return this.Title.Equals(menu.Title);
        }

        public override int GetHashCode()
        {
            return this.Id.HasValue ? this.Id.Value.GetHashCode() : this.Title.GetHashCode();
        }
    }
}