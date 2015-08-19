namespace KitchenPC.Recipes
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using KitchenPC.Ingredients;
    using KitchenPC.Recipes.Enums;

    public class Recipe
    {
        private List<IngredientUsage> ingredients;
        private Guid id;
        private string title;
        private string description;
        private string imageUrl;
        private string credit;
        private string creditUrl;
        private string permalink;
        private string method;
        private short prepTime;
        private short cookTime;
        private short averageRating;
        private short servingSize;
        private int comments;
        private RecipeTags tags;
        private int ingredientMenus;

        public Recipe(Guid id, RecipeTags tags, IngredientUsage[] ingredients)
        {
            this.ingredients = new IngredientUsageCollection();

            this.Id = id;
            this.Tags = tags;
            this.Ingredients = ingredients;
        }

        public Recipe()
        {
        }

        public Guid Id
        {
            get
            {
                return this.id;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "The Recipe Id cannot be null.");
                }
                this.id = value;
            }
        }

        public Guid OwnerId { get; set; }

        public string OwnerAlias { get; set; }

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.CheckForValidString(value, "Title");
                this.title = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.CheckForValidString(value, "Description");
                if (value.Length > 250)
                {
                    value = value.Substring(0, 250) + "...";
                }
                this.description = value;
            }
        }

        public string ImageUrl
        {
            get
            {
                return this.imageUrl;
            }

            set
            {
                this.CheckForValidString(value, "Image Url");
                this.imageUrl = value;
            }
        }

        public string Credit
        {
            get
            {
                return this.credit;
            }

            set
            {
                this.CheckForValidString(value, "Credit");
                this.credit = value;
            }
        }

        public string CreditUrl
        {
            get
            {
                return this.creditUrl;
            }

            set
            {
                this.CheckForValidString(value, "Credit Url");
                string pattern = @"^https\:\/\/(?<domain>[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3})(\/\S*)?$";
                var m = Regex.Match(value, pattern);
                if (m.Success)
                {
                    this.creditUrl = m.Value;
                }
                else
                {
                    throw new InvalidRecipeDataException(
                        "Credit Url does not match expected pattern (https://<domain name>.<ending>)");
                }
            }
        }

        public string Permalink
        {
            get
            {
                return this.permalink;
            }

            set
            {
                this.CheckForValidString(value, "Permalink");
                this.permalink = value;
            }
        }

        public string Method
        {
            get
            {
                return this.method;
            }

            set
            {
                this.CheckForValidString(value, "Method");
                this.method = value;
            }
        }

        public DateTime DateEntered { get; set; }

        public short PrepTime
        {
            get
            {
                return this.prepTime;
            }

            set
            {
                if (value < 0)
                {
                    throw new InvalidRecipeDataException("The Recipe Prepare Time must be equal to or greater than zero.");
                }
                this.prepTime = value;
            }
        }

        public short CookTime
        {
            get
            {
                return this.cookTime;
            }

            set
            {
                if (value < 0)
                {
                    throw new InvalidRecipeDataException("The Recipe Cook Time must be equal to or greater than zero.");
                }
                this.cookTime = value;
            }
        }

        public short AvgRating
        {
            get
            {
                return this.averageRating;
            }

            set
            {
                if (value < 0)
                {
                    throw new InvalidRecipeDataException("The Recipe Average Rating must be equal to or greater than zero.");
                }

                this.averageRating = value;
            }
        }

        public short ServingSize
        {
            get
            {
                return this.servingSize;
            }

            set
            {
                if (value <= 0)
                {
                    throw new InvalidRecipeDataException("The Recipe Serving Size must be greater than zero.");
                }

                this.servingSize = value;
            }
        }

        public Rating UserRating { get; set; }

        public int Comments
        {
            get
            {
                return this.comments;
            }

            set
            {
                if (value < 0)
                {
                    throw new InvalidRecipeDataException("The Recipe Comments cannot be less than zero.");
                }
                this.comments = value;
            }
        }

        public RecipeTags Tags
        {
            get
            {
                return this.tags;
            }

            set
            {
                if (value == null || value.Length <= 0)
                {
                    throw new InvalidRecipeDataException("Recipes must contain at least one tag.");
                }

                this.tags = value;
            }
        }

        public bool PublicEdit { get; set; }

        public bool AllowDelete { get; set; }

        public int InMenus
        {
            get
            {
                return this.ingredientMenus;
            }

            set
            {
                if (value < 0)
                {
                    throw new InvalidRecipeDataException("The Recipe Ingredient Menus cannot be less than zero.");
                }

                this.ingredientMenus = value;
            }
        }

        public IngredientUsage[] Ingredients
        {
            get
            {
                return this.ingredients.ToArray();
            }

            set
            {
                if (value == null || value.Length <= 0)
                {
                    throw new InvalidRecipeDataException("Recipes must contain at least one ingredient.");
                }
                this.ingredients = new List<IngredientUsage>(value);
            }
        }

        public void AddIngredient(IngredientUsage ingredient)
        {
            this.ingredients.Add(ingredient);
        }

        public void AddIngredients(IEnumerable<IngredientUsage> ingredientstoAdd)
        {
            this.ingredients.AddRange(ingredientstoAdd);
        }

        //DRY Principle - This Validation is used by a lot of the properties therefore we have extracted it into a method so it can be reused.
        private void CheckForValidString(string value, string name)
        {
            if (value == null || value.Trim() == string.Empty)
            {
                throw new InvalidRecipeDataException(string.Format("Recipe {0} cannot be null or empty.", name));
            }
        }
    }
}