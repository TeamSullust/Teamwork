using System;

namespace KitchenPC
{
    public class Weight : IComparable, IFormattable, IComparable<int>, IEquatable<int>
    {
        public readonly int Value;

        public Weight()
        {
            this.Value = 0;
        }

        public Weight(int grams)
        {
            this.Value = grams;
        }

        public static implicit operator Weight(int grams)
        {
            return new Weight(grams);
        }

        public static implicit operator int(Weight weight)
        {
            return weight == null ? 0 : weight.Value;
        }

        public static bool operator ==(Weight x, Weight y)
        {
            var first = x as object;
            var second = y as object;

            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (first == null || second == null)
            {
                return false;
            }

            return x.Value == y.Value;
        }

        public static bool operator !=(Weight x, Weight y)
        {
            return !(x == y);
        }

        public int CompareTo(object obj)
        {
            var weight = obj as Weight;
            return weight != null ? this.Value.CompareTo(weight.Value) : this.Value.CompareTo(obj);
        }

        public int CompareTo(int other)
        {
            return this.Value.CompareTo(other);
        }

        public bool Equals(int other)
        {
            return this.Value.Equals(other);
        }

        public override bool Equals(object obj)
        {
            var weight = obj as Weight;
            if (weight != null)
            {
                return this.Value == weight.Value;
            }

            return false;
        }

        public override string ToString()
        {
            return string.Format("{0:f} g.", this.Value);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString();
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }
}