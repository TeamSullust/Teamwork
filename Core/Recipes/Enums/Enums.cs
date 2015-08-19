namespace KitchenPC.Recipes.Enums
{
   public enum MealFilter
   {
      All,
      Breakfast,
      Lunch,
      Dinner,
      Dessert
   }

   public enum Rating
   {
      None = 0,

      OneStar = 1,
      TwoStars = 2,
      ThreeStars = 3,
      FourStars = 4,
      FiveStars = 5
   }

   public enum PhotoFilter
   {
       All,
       Photo,
       HighRes
   }

   public enum SortOrder
   {
       None,
       Title,
       PrepTime,
       CookTime,
       Rating,
       Image
   }

   public enum SortDirection
   {
       Ascending,
       Descending
   }

   public enum SpicinessLevel
   {
       Mild,
       MildMedium,
       Medium,
       MediumSpicy,
       Spicy
   }

   public enum SweetnessLevel
   {
       Savory,
       SavoryMedium,
       Medium,
       MediumSweet,
       Sweet
   }
}