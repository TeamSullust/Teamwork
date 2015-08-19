namespace KitchenPC.Recipes.Filters
{
    using System;

    public class SkillFilter : ICloneable, IEquatable< SkillFilter>
    {
        public bool Common { get; set; }

        public bool Easy { get; set; }

        public bool Quick { get; set; }

        public static implicit operator bool(SkillFilter f)
        {
            return f.Common || f.Easy || f.Quick;
        }

        public object Clone()
        {
            var clonedSkillFilter = new SkillFilter { Common = this.Common, Easy = this.Easy, Quick = this.Quick };
            return clonedSkillFilter;
        }

        public bool Equals(SkillFilter other)
        {
            if (object.Equals(other, null))
            {
                return false;
            }
            return this.Common == other.Common && this.Easy == other.Easy && this.Quick == other.Quick;
        }
    }
}
