namespace KitchenPC.Recipes
{
    using System;
    using System.Collections.Generic;

    using KitchenPC.Recipes.Enums;
    using KitchenPC.Recipes.Filters;

    public class RecipeQuery : ICloneable
    {
        private TimeFilter time;

        private DietFilter diet;

        private NutritionFilter nutrition;

        private SkillFilter skill;

        private TasteFilter tase;

        private Guid[] include;

        private Guid[] exclude;

        public RecipeQuery()
        {
            this.Time = new TimeFilter();
            this.Diet = new DietFilter();
            this.Nutrition = new NutritionFilter();
            this.Skill = new SkillFilter();
            this.Taste = new TasteFilter();
            this.Sort = SortOrder.Rating;
            this.Direction = SortDirection.Descending;
            this.Include = new Guid[0];
            this.Exclude = new Guid[0];
        }

        public string Keywords { get; set; }

        public MealFilter Meal { get; set; }

        public TimeFilter Time
        {
            get
            {
                return this.time;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Recipe Query Time cannot be null.");
                }

                this.time = value;
            }
        }

        public DietFilter Diet
        {
            get
            {
                return this.diet;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Recipe Query Diet cannot be null.");
                }

                this.diet = value;
            }
        }

        public NutritionFilter Nutrition
        {
            get
            {
                return this.nutrition;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Recipe Query Nutrition cannot be null.");
                }

                this.nutrition = value;
            }
        }

        public SkillFilter Skill
        {
            get
            {
                return this.skill;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Recipe Query Skill cannot be null.");
                }

                this.skill = value;
            }
        }

        public TasteFilter Taste
        {
            get
            {
                return this.tase;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Recipe Query Taste cannot be null.");
                }

                this.tase = value;
            }
        }

        public Rating Rating { get; set; }

        public Guid[] Include
        {
            get
            {
                return this.include;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value","Recipe Query Include cannot be null.");
                }

                this.include = value;
            }
        }

        public Guid[] Exclude
        {
            get
            {
                return this.exclude;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value","Recipe Query Exclude cannot be null.");
                }

                this.exclude = value;
            }
        }

        public int Offset { get; set; } // Used for paging

        public PhotoFilter Photos { get; set; }

        public SortOrder Sort { get; set; }

        public SortDirection Direction { get; set; }

        public object Clone()
        {
            var clonedQuery = new RecipeQuery()
                                  {
                                      Keywords = this.Keywords,
                                      Rating = this.Rating,
                                      Meal = this.Meal,
                                      Offset = this.Offset,
                                      Photos = this.Photos,
                                      Sort = this.Sort,
                                      Direction = this.Direction,
                                      Time = this.Time.Clone() as TimeFilter,
                                      Diet = this.Diet.Clone() as DietFilter,
                                      Nutrition = this.Nutrition.Clone() as NutritionFilter,
                                      Skill = this.Skill.Clone() as SkillFilter,
                                      Taste = this.Taste.Clone() as TasteFilter
                                  };

            if (this.Include != null)
            {
                clonedQuery.Include = this.Include.Clone() as Guid[];
            }

            if (this.Exclude != null)
            {
                clonedQuery.Exclude = this.Exclude.Clone() as Guid[];
            }

            return clonedQuery;
        }
    }
}