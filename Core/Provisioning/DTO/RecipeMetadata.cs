using System;
using KitchenPC.Recipes;

namespace KitchenPC.Data.DTO
{
    using KitchenPC.Recipes.Enums;

    public class RecipeMetadata
   {
      public Guid RecipeMetadataId { get; set; }
      public Guid RecipeId { get; set; }

      public int PhotoRes { get; set; }
      public float Commonality { get; set; }
      public bool UsdaMatch { get; set; }

      public bool MealBreakfast { get; set; }
      public bool MealLunch { get; set; }
      public bool MealDinner { get; set; }
      public bool MealDessert { get; set; }

      public bool DietNomeat { get; set; }
      public bool DietGlutenFree { get; set; }
      public bool DietNoRedMeat { get; set; }
      public bool DietNoAnimals { get; set; }
      public bool DietNoPork { get; set; }

      public short NutritionTotalfat { get; set; }
      public short NutritionTotalSodium { get; set; }
      public bool NutritionLowSodium { get; set; }
      public bool NutritionLowSugar { get; set; }
      public bool NutritionLowCalorie { get; set; }
      public short NutritionTotalSugar { get; set; }
      public short NutritionTotalCalories { get; set; }
      public bool NutritionLowFat { get; set; }
      public bool NutritionLowCarb { get; set; }
      public short NutritionTotalCarbs { get; set; }

      public bool SkillQuick { get; set; }
      public bool SkillEasy { get; set; }
      public bool SkillCommon { get; set; }

      public short TasteMildToSpicy { get; set; }
      public short TasteSavoryToSweet { get; set; }

      public static RecipeTags ToRecipeTags(RecipeMetadata metadata)
      {
          var tags = new RecipeTags();

          if (metadata.DietGlutenFree)
          {
              tags.Add(RecipeTag.GlutenFree);
          }

          if (metadata.DietNoAnimals)
          {
              tags.Add(RecipeTag.NoAnimals);
          }

          if (metadata.DietNomeat)
          {
              tags.Add(RecipeTag.NoMeat);
          }

          if (metadata.DietNoPork)
          {
              tags.Add(RecipeTag.NoPork);
          }

          if (metadata.DietNoRedMeat)
          {
              tags.Add(RecipeTag.NoRedMeat);
          }

          if (metadata.MealBreakfast)
          {
              tags.Add(RecipeTag.Breakfast);
          }

          if (metadata.MealDessert)
          {
              tags.Add(RecipeTag.Dessert);
          }

          if (metadata.MealDinner)
          {
              tags.Add(RecipeTag.Dinner);
          }

          if (metadata.MealLunch)
          {
              tags.Add(RecipeTag.Lunch);
          }

          if (metadata.NutritionLowCalorie)
          {
              tags.Add(RecipeTag.LowCalorie);
          }

          if (metadata.NutritionLowCarb)
          {
              tags.Add(RecipeTag.LowCarb);
          }

          if (metadata.NutritionLowFat)
          {
              tags.Add(RecipeTag.LowFat);
          }

          if (metadata.NutritionLowSodium)
          {
              tags.Add(RecipeTag.LowSodium);
          }

          if (metadata.NutritionLowSugar)
          {
              tags.Add(RecipeTag.LowSugar);
          }

          if (metadata.SkillCommon)
          {
              tags.Add(RecipeTag.CommonIngredients);
          }

          if (metadata.SkillEasy)
          {
              tags.Add(RecipeTag.EasyToMake);
          }

          if (metadata.SkillQuick)
          {
              tags.Add(RecipeTag.Quick);
          }

          return tags;
      }
   }
}