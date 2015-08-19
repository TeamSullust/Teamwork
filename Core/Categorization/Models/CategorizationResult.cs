namespace KitchenPC.Categorization.Models
{
   public class CategorizationResult
   {
       public bool DietGlutenFree { get; set; }

       public bool DietNoAnimals { get; set; }

       public bool DietNoMeat { get; set; }

       public bool DietNoPork { get; set; }

       public bool DietNoRedMeat { get; set; }

       public bool MealBreakfast { get; set; }

       public bool MealDessert { get; set; }

       public bool MealDinner { get; set; }

       public bool MealLunch { get; set; }

       public bool NutritionLowCalorie { get; set; }

       public bool NutritionLowCarb { get; set; }

       public bool NutritionLowFat { get; set; }

       public bool NutritionLowSodium { get; set; }

       public bool NutritionLowSugar { get; set; }

       public short NutritionTotalCalories { get; set; }

       public short NutritionTotalCarbs { get; set; }

       public short NutritionTotalFat { get; set; }

       public short NutritionTotalSodium { get; set; }

       public short NutritionTotalSugar { get; set; }

       public bool SkillEasy { get; set; }

       public bool SkillQuick { get; set; }

       public bool SkillCommon { get; set; }

       public byte TasteMildToSpicy { get; set; }

       public byte TasteSavoryToSweet { get; set; }

       public float Commonality { get; set; }

       public bool USDAMatch { get; set; }
   }
}