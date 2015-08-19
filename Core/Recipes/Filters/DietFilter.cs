namespace KitchenPC.Recipes.Filters
{
    using System;

    public class DietFilter : ICloneable, IEquatable<DietFilter>
    {
        public bool GlutenFree { get; set; }

        public bool NoAnimals { get; set; }

        public bool NoMeat { get; set; }

        public bool NoPork { get; set; }

        public bool NoRedMeat { get; set; }

        public static implicit operator bool(DietFilter f)
        {
            return f.GlutenFree || f.NoAnimals || f.NoMeat || f.NoPork || f.NoRedMeat;
        }

        public object Clone()
        {
            var clonedDietFilter = new DietFilter
                                       {
                                           GlutenFree = this.GlutenFree,
                                           NoAnimals = this.NoAnimals,
                                           NoMeat = this.NoMeat,
                                           NoPork = this.NoPork,
                                           NoRedMeat = this.NoRedMeat
                                       };
            return clonedDietFilter;
        }

        public bool Equals(DietFilter other)
        {
            if (object.Equals(other, null))
            {
                return false;
            }

            return
                this.GlutenFree == other.GlutenFree && this.NoAnimals == other.NoAnimals && this.NoMeat ==
                 other.NoMeat && this.NoPork == other.NoPork && this.NoRedMeat == other.NoRedMeat;
        }
    }
}
