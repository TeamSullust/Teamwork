namespace KitchenPC.Categorization
{
    internal class TimeToken : IToken
    {
        private enum Classification
        {
            Quick,

            Medium,

            Long,

            SuperLong
        };

        private readonly Classification classification;

        public TimeToken(int minutes)
        {
            if (minutes < 10)
            {
                classification = Classification.Quick;
            }
            else if (minutes < 30)
            {
                classification = Classification.Medium;
            }
            else if (minutes <= 60)
            {
                classification = Classification.Long;
            }
            else
            {
                classification = Classification.SuperLong;
            }
        }

        public override bool Equals(object obj)
        {
            var comparedToken = obj as TimeToken;
            return comparedToken != null && comparedToken.classification.Equals(classification);
        }

        public override int GetHashCode()
        {
            return classification.GetHashCode();
        }

        public override string ToString()
        {
            return classification.ToString();
        }
    }
}