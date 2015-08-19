namespace KitchenPC.Recipes.Filters
{
    using System;

    public class NutritionFilter : ICloneable,IEquatable<NutritionFilter>
    {
        public bool LowCalorie { get; set; }

        public bool LowCarb { get; set; }

        public bool LowFat { get; set; }

        public bool LowSodium { get; set; }

        public bool LowSugar { get; set; }

        public static implicit operator bool(NutritionFilter f)
        {
            return f.LowCalorie || f.LowCarb || f.LowFat || f.LowSodium || f.LowSugar;
        }

        public object Clone()
        {
            var clonedNutritionFilter = new NutritionFilter
                                            {
                                                LowCalorie = this.LowCalorie,
                                                LowCarb = this.LowCarb,
                                                LowFat = this.LowFat,
                                                LowSodium = this.LowSodium,
                                                LowSugar = this.LowSugar
                                            };
            return clonedNutritionFilter;
        }

        public bool Equals(NutritionFilter other)
        {
            if (object.Equals(other, null))
            {
                return false;
            }
            return this.LowCalorie == other.LowCalorie && this.LowCarb == other.LowCarb && this.LowFat == other.LowFat
                   && this.LowSodium == other.LowSodium && this.LowSugar == other.LowSugar;
        }
    }
}
