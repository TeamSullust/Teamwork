using System;
using System.Linq;
using KitchenPC.Data;
using KitchenPC.Recipes;

namespace KitchenPC.Context
{
    using KitchenPC.Recipes.Enums;

    internal class StaticSearch : ISearchProvider
   {
      readonly DataStore store;

      public StaticSearch(DataStore store)
      {
         this.store = store;
      }

      public SearchResults Search(AuthIdentity identity, RecipeQuery query)
      {
         var index = store.GetSearchIndex();

         var q = index.Where(p => !p.Recipe.Hidden);

         if (!String.IsNullOrWhiteSpace(query.Keywords))
         {
            q = q.Where(p =>
               (p.Recipe.Title ?? "").IndexOf(query.Keywords, StringComparison.OrdinalIgnoreCase) >= 0 ||
               (p.Recipe.Description ?? "").IndexOf(query.Keywords, StringComparison.OrdinalIgnoreCase) >= 0
               );
         }

         if (query.Time.MaxPrep.HasValue)
         {
            q = q.Where(p => p.Recipe.PrepTime <= query.Time.MaxPrep.Value);
         }

         if (query.Time.MaxCook.HasValue)
         {
            q = q.Where(p => p.Recipe.CookTime <= query.Time.MaxCook.Value);
         }

         if (query.Rating > 0)
         {
            q = q.Where(p => p.Recipe.Rating >= (int) query.Rating);
         }

         if (query.Include != null && query.Include.Length > 0) // Add ingredients to include
         {
            q = q.Where(p => p.Ingredients.Any(i => query.Include.Contains(i.IngredientId)));
         }

         if (query.Exclude != null && query.Exclude.Length > 0) // Add ingredients to exclude
         {
            q = q.Where(p => !p.Ingredients.Any(i => query.Exclude.Contains(i.IngredientId)));
         }

         if (query.Photos == PhotoFilter.Photo || query.Photos == PhotoFilter.HighRes)
         {
            q = q.Where(p => p.Recipe.ImageUrl != null);
         }

         if (query.Diet || query.Nutrition || query.Skill || query.Taste || (query.Meal != MealFilter.All) || (query.Photos == PhotoFilter.HighRes)) //Need to search in metadata
         {
            //Meal
            if (query.Meal != MealFilter.All)
            {
               if (query.Meal == MealFilter.Breakfast) q = q.Where(p => p.Metadata.MealBreakfast);
               if (query.Meal == MealFilter.Dessert) q = q.Where(p => p.Metadata.MealDessert);
               if (query.Meal == MealFilter.Dinner) q = q.Where(p => p.Metadata.MealDinner);
               if (query.Meal == MealFilter.Lunch) q = q.Where(p => p.Metadata.MealLunch);
            }

            //High-res photos
            if (query.Photos == PhotoFilter.HighRes) q = q.Where(p => p.Metadata.PhotoRes >= 1024*768);

            //Diet
            if (query.Diet.GlutenFree) q = q.Where(p => p.Metadata.DietGlutenFree);
            if (query.Diet.NoAnimals) q = q.Where(p => p.Metadata.DietNoAnimals);
            if (query.Diet.NoMeat) q = q.Where(p => p.Metadata.DietNomeat);
            if (query.Diet.NoPork) q = q.Where(p => p.Metadata.DietNoPork);
            if (query.Diet.NoRedMeat) q = q.Where(p => p.Metadata.DietNoRedMeat);

            //Nutrition
            if (query.Nutrition.LowCalorie) q = q.Where(p => p.Metadata.NutritionLowCalorie);
            if (query.Nutrition.LowCarb) q = q.Where(p => p.Metadata.NutritionLowCarb);
            if (query.Nutrition.LowFat) q = q.Where(p => p.Metadata.NutritionLowFat);
            if (query.Nutrition.LowSodium) q = q.Where(p => p.Metadata.NutritionLowSodium);
            if (query.Nutrition.LowSugar) q = q.Where(p => p.Metadata.NutritionLowSugar);

            //Skill
            if (query.Skill.Common) q = q.Where(p => p.Metadata.SkillCommon).OrderByDescending(p => p.Metadata.Commonality);
            if (query.Skill.Easy) q = q.Where(p => p.Metadata.SkillEasy);
            if (query.Skill.Quick) q = q.Where(p => p.Metadata.SkillQuick);

            //Taste
            if (query.Taste.MildToSpicy != SpicinessLevel.Medium)
            {
               q = query.Taste.MildToSpicy < SpicinessLevel.Medium
                  ? q.Where(p => p.Metadata.TasteMildToSpicy <= query.Taste.Spiciness).OrderBy(p => p.Metadata.TasteMildToSpicy)
                  : q.Where(p => p.Metadata.TasteMildToSpicy >= query.Taste.Spiciness).OrderByDescending(p => p.Metadata.TasteMildToSpicy);
            }

            if (query.Taste.SavoryToSweet != SweetnessLevel.Medium)
            {
               q = query.Taste.SavoryToSweet < SweetnessLevel.Medium
                  ? q.Where(p => p.Metadata.TasteSavoryToSweet <= query.Taste.Sweetness).OrderBy(p => p.Metadata.TasteSavoryToSweet)
                  : q.Where(p => p.Metadata.TasteSavoryToSweet >= query.Taste.Sweetness).OrderByDescending(p => p.Metadata.TasteSavoryToSweet);
            }
         }

         switch (query.Sort)
         {
            case SortOrder.Title:
               q = (query.Direction == SortDirection.Ascending) ? q.OrderBy(p => p.Recipe.Title) : q.OrderByDescending(p => p.Recipe.Title);
               break;
            case SortOrder.PrepTime:
               q = (query.Direction == SortDirection.Ascending) ? q.OrderBy(p => p.Recipe.PrepTime) : q.OrderByDescending(p => p.Recipe.PrepTime);
               break;
            case SortOrder.CookTime:
               q = (query.Direction == SortDirection.Ascending) ? q.OrderBy(p => p.Recipe.CookTime) : q.OrderByDescending(p => p.Recipe.CookTime);
               break;
            case SortOrder.Image:
               q = (query.Direction == SortDirection.Ascending) ? q.OrderBy(p => p.Recipe.ImageUrl) : q.OrderByDescending(p => p.Recipe.ImageUrl);
               break;
            default:
               q = (query.Direction == SortDirection.Ascending) ? q.OrderBy(p => p.Recipe.Rating) : q.OrderByDescending(p => p.Recipe.Rating);
               break;
         }

         return new SearchResults
         {
            Briefs = q.Select(r => Data.DTO.Recipes.ToRecipeBrief(r.Recipe)).ToArray(),
            TotalCount = q.Count()
         };
      }
   }
}