namespace KitchenPC.Categorization.Models.Tokens
{
    using KitchenPC.Categorization.Enums;
    using KitchenPC.Categorization.Interfaces;

    public class TimeToken : IToken
    {
        public TimeToken(int minutes)
        {
            if (minutes < 10)
            {
                this.Time = Classification.Quick;
            }
            else if (minutes < 30)
            {
                this.Time = Classification.Medium;
            }
            else if (minutes <= 60)
            {
                this.Time = Classification.Long;
            }
            else
            {
                this.Time = Classification.SuperLong;
            }
        }

        public Classification Time { get; private set; }

        public override bool Equals(object obj)
        {
            var t1 = obj as TimeToken;
            return t1 != null && t1.Time.Equals(this.Time);
        }

        public override int GetHashCode()
        {
            return this.Time.GetHashCode();
        }

        public override string ToString()
        {
            return this.Time.ToString();
        }
    }
}