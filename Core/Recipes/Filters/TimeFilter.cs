namespace KitchenPC.Recipes.Filters
{
    using System;

    public class TimeFilter : ICloneable, IEquatable<TimeFilter>
    {
        public short? MaxPrep { get; set; }

        public short? MaxCook { get; set; }

        public static implicit operator bool(TimeFilter f)
        {
            return f.MaxPrep.HasValue || f.MaxCook.HasValue;
        }

        public object Clone()
        {
            var clonedTimeFilter = new TimeFilter();
            if (this.MaxPrep != null)
            {
                clonedTimeFilter.MaxPrep = this.MaxPrep;
            }

            if (this.MaxCook != null)
            {
                clonedTimeFilter.MaxCook = this.MaxCook;
            }

            return clonedTimeFilter;
        }

        public bool Equals(TimeFilter other)
        {
            if (object.Equals(other, null))
            {
                return false;
            }
            return this.MaxCook == other.MaxCook && this.MaxPrep == other.MaxPrep;
        }
    }
}
