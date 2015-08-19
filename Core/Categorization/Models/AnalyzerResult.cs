namespace KitchenPC.Categorization.Models
{
    using KitchenPC.Categorization.Enums;

    public class AnalyzerResult
   {
      public AnalyzerResult(Category first, Category second)
      {
         this.FirstPlace = first;
         this.SecondPlace = second;
      }

      public Category FirstPlace { get; private set; }

      public Category SecondPlace { get; private set; }

      public override string ToString()
      {
         return this.SecondPlace == Category.None ? this.FirstPlace.ToString()
            : string.Format("{0}/{1}", this.FirstPlace, this.SecondPlace);
      }
   }
}