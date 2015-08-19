namespace KitchenPC.Fluent.RecipeHandlers
{
    using System.Linq;
    using KitchenPC.Ingredients;
    using KitchenPC.Recipes;
    using KitchenPC.Recipes.Enums;

    /// <summary>Provides the ability to fluently build a search query.</summary>
    public class RecipeQueryBuilder
    {
        private readonly RecipeQuery query;

        public RecipeQueryBuilder(RecipeQuery query)
        {
            this.query = query;
        }

        public RecipeQueryBuilder GlutenFree
        {
            get
            {
                this.query.Diet.GlutenFree = true;
                return this;
            }
        }

        public RecipeQueryBuilder NoAnimals
        {
            get
            {
                this.query.Diet.NoAnimals = true;
                return this;
            }
        }

        public RecipeQueryBuilder NoMeat
        {
            get
            {
                this.query.Diet.NoMeat = true;
                return this;
            }
        }

        public RecipeQueryBuilder NoPork
        {
            get
            {
                this.query.Diet.NoPork = true;
                return this;
            }
        }

        public RecipeQueryBuilder NoRedMeat
        {
            get
            {
                this.query.Diet.NoRedMeat = true;
                return this;
            }
        }

        public RecipeQueryBuilder LowCalorie
        {
            get
            {
                this.query.Nutrition.LowCalorie = true;
                return this;
            }
        }

        public RecipeQueryBuilder LowCarb
        {
            get
            {
                this.query.Nutrition.LowCarb = true;
                return this;
            }
        }

        public RecipeQueryBuilder LowFat
        {
            get
            {
                this.query.Nutrition.LowFat = true;
                return this;
            }
        }

        public RecipeQueryBuilder LowSodium
        {
            get
            {
                this.query.Nutrition.LowSodium = true;
                return this;
            }
        }

        public RecipeQueryBuilder LowSugar
        {
            get
            {
                this.query.Nutrition.LowSugar = true;
                return this;
            }
        }

        public RecipeQueryBuilder Common
        {
            get
            {
                this.query.Skill.Common = true;
                return this;
            }
        }

        public RecipeQueryBuilder Easy
        {
            get
            {
                this.query.Skill.Easy = true;
                return this;
            }
        }

        public RecipeQueryBuilder Quick
        {
            get
            {
                this.query.Skill.Quick = true;
                return this;
            }
        }

        public RecipeQuery Query
        {
            get
            {
                return this.query;
            }
        }

        public RecipeQueryBuilder Keywords(string keywords)
        {
            this.query.Keywords = keywords;
            return this;
        }

        public RecipeQueryBuilder MinRating(Rating rating)
        {
            this.query.Rating = rating;
            return this;
        }

        public RecipeQueryBuilder IncludeIngredients(params Ingredient[] ingredients)
        {
            this.query.Include = ingredients.Select(i => i.Id).ToArray();
            return this;
        }

        public RecipeQueryBuilder ExcludeIngredients(params Ingredient[] ingredients)
        {
            this.query.Exclude = ingredients.Select(i => i.Id).ToArray();
            return this;
        }

        public RecipeQueryBuilder Offset(int startOffset)
        {
            this.query.Offset = startOffset;
            return this;
        }

        public RecipeQueryBuilder Meal(MealFilter mealFilter)
        {
            this.query.Meal = mealFilter;
            return this;
        }

        public RecipeQueryBuilder HasPhoto(PhotoFilter photoFilter)
        {
            this.query.Photos = photoFilter;
            return this;
        }

        public RecipeQueryBuilder SortBy(SortOrder sortOrder, SortDirection direction = SortDirection.Ascending)
        {
            this.query.Sort = sortOrder;
            this.query.Direction = direction;
            return this;
        }

        public RecipeQueryBuilder MaxPrep(short minutes)
        {
            this.query.Time.MaxPrep = minutes;
            return this;
        }

        public RecipeQueryBuilder MaxCook(short minutes)
        {
            this.query.Time.MaxCook = minutes;
            return this;
        }

        public RecipeQueryBuilder MildToSpicy(SpicinessLevel scale)
        {
            this.query.Taste.MildToSpicy = scale;
            return this;
        }

        public RecipeQueryBuilder SavoryToSweet(SweetnessLevel scale)
        {
            this.query.Taste.SavoryToSweet = scale;
            return this;
        }
    }
}
