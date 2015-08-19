namespace KitchenPC.Recipes.Filters
{
    using System;

    using KitchenPC.Recipes.Enums;

    public class TasteFilter : ICloneable, IEquatable<TasteFilter>
    {
        private static readonly byte[] SpicyOffsets = { 0, 2, 0, 3, 10 };

        private static readonly byte[] SweetOffsets = { 3, 10, 0, 20, 30 };

        public TasteFilter()
        {
            this.MildToSpicy = SpicinessLevel.Medium;
            this.SavoryToSweet = SweetnessLevel.Medium;
        }

        public SpicinessLevel MildToSpicy { get; set; }

        public SweetnessLevel SavoryToSweet { get; set; }


        public byte Spiciness
        {
            get
            {
                return SpicyOffsets[(int)this.MildToSpicy];
            }
        }

        public byte Sweetness
        {
            get
            {
                return SweetOffsets[(int)this.SavoryToSweet];
            }
        }

        public static implicit operator bool(TasteFilter f)
        {
            return f.MildToSpicy != SpicinessLevel.Medium || f.SavoryToSweet != SweetnessLevel.Medium;
        }

        public object Clone()
        {
            var clonedTasteFilter = new TasteFilter()
                                        {
                                            MildToSpicy = this.MildToSpicy,
                                            SavoryToSweet = this.SavoryToSweet
                                        };
            return clonedTasteFilter;
        }

        public bool Equals(TasteFilter other)
        {
            if (object.Equals(other, null))
            {
                return false;
            }
            return this.MildToSpicy == other.MildToSpicy && this.SavoryToSweet == other.SavoryToSweet;
        }
    }
}
