namespace KitchenPC.Categorization
{
   internal class TextToken : IToken
   {
      readonly string text;

      public TextToken(string text)
      {
         this.text = text.Trim().ToLower();
      }

      public override bool Equals(object obj)
      {
         var comparedToken = obj as TextToken;
         return comparedToken != null && comparedToken.text.Equals(text);
      }

      public override int GetHashCode()
      {
         return text.GetHashCode();
      }

      public override string ToString()
      {
         return text;
      }
   }
}