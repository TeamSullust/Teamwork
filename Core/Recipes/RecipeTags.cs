namespace KitchenPC.Recipes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using KitchenPC.Recipes.Enums;

    public class RecipeTags : IEnumerable<RecipeTag>
    {
        private readonly List<RecipeTag> tags; // Enumerable list of RecipeTag objects

        public RecipeTags(params RecipeTag[] tags)
        {
            this.tags = new List<RecipeTag>(tags.Distinct());
        }

        public static RecipeTags None
        {
            get
            {
                return new RecipeTags();
            }
        }

        public int Length
        {
            get
            {
                return this.tags.Count;
            }
        }

        public RecipeTag this[int index]
        {
            get
            {
                if (index > this.tags.Count || index < 0)
                {
                    throw new IndexOutOfRangeException("RecipeTag index must be within the bounds of the array.");
                }

                return this.tags[index];
            }
        }

        public static RecipeTags Parse(string list)
        {
            var tagsAsString = list.Split(',').Select(t => t.Replace(" ", string.Empty));
            var tags = tagsAsString.Select(t => (RecipeTag)Enum.Parse(typeof(RecipeTag), t)).Distinct().ToArray();
            var recipeTags = new RecipeTags(tags);
            return recipeTags;
        }

        public static bool operator !=(RecipeTags x, RecipeTags y)
        {
            return !(x == y);
        }

        public static bool operator ==(RecipeTags x, RecipeTags y)
        {
            if (object.Equals(x, null) && object.Equals(y, null))
            {
                return true;
            }
            else if (object.Equals(x, null) || object.Equals(y, null))
            {
                return false;
            }

                return ReferenceEquals(x, y);
        }

        public bool HasTag(RecipeTag tag)
        {
            return this.tags.Contains(tag);
        }

        public override bool Equals(object other)
        {
            var otherRecipeTags = other as RecipeTags;
            return this == otherRecipeTags;
        }

        public override string ToString()
        {
            return string.Join(", ", this.tags);
        }

        public override int GetHashCode()
        {
            return this.tags.GetHashCode();
        }

        public IEnumerator<RecipeTag> GetEnumerator()
        {
            return this.tags.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.tags.GetEnumerator();
        }

        public void Add(RecipeTag tag)
        {
            if (!this.HasTag(tag))
            {
                this.tags.Add(tag);
            }
        }
    }
}