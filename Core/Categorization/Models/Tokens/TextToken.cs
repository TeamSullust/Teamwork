namespace KitchenPC.Categorization.Models.Tokens
{
    using KitchenPC.Categorization.Interfaces;

    public class TextToken : IToken
    {
        public TextToken(string text)
        {
            this.Text = text.Trim().ToLower();
        }

        public string Text { get; private set; }

        public override bool Equals(object obj)
        {
            var t1 = obj as TextToken;
            return t1 != null && t1.Text.Equals(this.Text);
        }

        public override int GetHashCode()
        {
            return this.Text.GetHashCode();
        }

        public override string ToString()
        {
            return this.Text;
        }
    }
}