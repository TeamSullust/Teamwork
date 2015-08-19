namespace KitchenPC.DB.Models
{
    using System;
    using FluentNHibernate.Mapping;
    using KitchenPC.Recipes;
    using KitchenPC.Recipes.Enums;

    public class RecipeMetadata
   {
      public virtual Guid RecipeMetadataId { get; set; }

      public virtual Recipes Recipe { get; set; }

      public virtual int PhotoRes { get; set; }

      public virtual float Commonality { get; set; }

      public virtual bool UsdaMatch { get; set; }


      public virtual bool MealBreakfast { get; set; }

      public virtual bool MealLunch { get; set; }

      public virtual bool MealDinner { get; set; }

      public virtual bool MealDessert { get; set; }


      public virtual bool DietNomeat { get; set; }

      public virtual bool DietGlutenFree { get; set; }

      public virtual bool DietNoRedMeat { get; set; }

      public virtual bool DietNoAnimals { get; set; }

      public virtual bool DietNoPork { get; set; }


      public virtual short NutritionTotalfat { get; set; }

      public virtual short NutritionTotalSodium { get; set; }

      public virtual bool NutritionLowSodium { get; set; }

      public virtual bool NutritionLowSugar { get; set; }

      public virtual bool NutritionLowCalorie { get; set; }

      public virtual short NutritionTotalSugar { get; set; }

      public virtual short NutritionTotalCalories { get; set; }

      public virtual bool NutritionLowFat { get; set; }

      public virtual bool NutritionLowCarb { get; set; }

      public virtual short NutritionTotalCarbs { get; set; }


      public virtual bool SkillQuick { get; set; }

      public virtual bool SkillEasy { get; set; }

      public virtual bool SkillCommon { get; set; }


      public virtual short TasteMildToSpicy { get; set; }

      public virtual short TasteSavoryToSweet { get; set; }

      public virtual RecipeTags Tags
      {
         get
         {
            var tags = new RecipeTags();

             if (this.DietGlutenFree)
             {
                 tags.Add(RecipeTag.GlutenFree);
             }

             if (this.DietNoAnimals)
             {
                 tags.Add(RecipeTag.NoAnimals);
             }

             if (this.DietNomeat)
             {
                 tags.Add(RecipeTag.NoMeat);
             }

             if (this.DietNoPork)
             {
                 tags.Add(RecipeTag.NoPork);
             }

             if (this.DietNoRedMeat)
             {
                 tags.Add(RecipeTag.NoRedMeat);
             }

             if (this.MealBreakfast)
             {
                 tags.Add(RecipeTag.Breakfast);
             }

             if (this.MealDessert)
             {
                 tags.Add(RecipeTag.Dessert);
             }

             if (this.MealDinner)
             {
                 tags.Add(RecipeTag.Dinner);
             }

             if (this.MealLunch)
             {
                 tags.Add(RecipeTag.Lunch);
             }

             if (this.NutritionLowCalorie)
             {
                 tags.Add(RecipeTag.LowCalorie);
             }

             if (this.NutritionLowCarb)
             {
                 tags.Add(RecipeTag.LowCarb);
             }

             if (this.NutritionLowFat)
             {
                 tags.Add(RecipeTag.LowFat);
             }

             if (this.NutritionLowSodium)
             {
                 tags.Add(RecipeTag.LowSodium);
             }

             if (this.NutritionLowSugar)
             {
                 tags.Add(RecipeTag.LowSugar);
             }

             if (this.SkillCommon)
             {
                 tags.Add(RecipeTag.CommonIngredients);
             }

             if (this.SkillEasy)
             {
                 tags.Add(RecipeTag.EasyToMake);
             }

             if (this.SkillQuick)
             {
                 tags.Add(RecipeTag.Quick);
             }

             return tags;
         }
      }

      public static RecipeMetadata FromId(Guid id)
      {
          return new RecipeMetadata
          {
              RecipeMetadataId = id
          };
      }
   }

   //public class RecipeMetadataMap : ClassMap<RecipeMetadata>
   //{
   //   public RecipeMetadataMap()
   //   {
   //      Id(x => x.RecipeMetadataId)
   //         .GeneratedBy.GuidComb()
   //         .UnsavedValue(Guid.Empty);

   //      Map(x => x.Commonality).Not.Nullable().Index("IDX_RecipeMetadata_Commonality");
   //      Map(x => x.DietGlutenFree).Not.Nullable().Index("IDX_RecipeMetadata_DietGlutenFree");
   //      Map(x => x.DietNoAnimals).Not.Nullable().Index("IDX_RecipeMetadata_DietNoAnimals");
   //      Map(x => x.DietNomeat).Not.Nullable().Index("IDX_RecipeMetadata_DietNomeat");
   //      Map(x => x.DietNoPork).Not.Nullable().Index("IDX_RecipeMetadata_DietNoPork");
   //      Map(x => x.DietNoRedMeat).Not.Nullable().Index("IDX_RecipeMetadata_DietNoRedMeat");
   //      Map(x => x.MealBreakfast).Not.Nullable().Index("IDX_RecipeMetadata_MealBreakfast");
   //      Map(x => x.MealDessert).Not.Nullable().Index("IDX_RecipeMetadata_MealDessert");
   //      Map(x => x.MealDinner).Not.Nullable().Index("IDX_RecipeMetadata_MealDinner");
   //      Map(x => x.MealLunch).Not.Nullable().Index("IDX_RecipeMetadata_MealLunch");
   //      Map(x => x.NutritionLowCalorie).Not.Nullable().Index("IDX_RecipeMetadata_NutritionLowCalorie");
   //      Map(x => x.NutritionLowCarb).Not.Nullable().Index("IDX_RecipeMetadata_NutritionLowCarb");
   //      Map(x => x.NutritionLowFat).Not.Nullable().Index("IDX_RecipeMetadata_NutritionLowFat");
   //      Map(x => x.NutritionLowSodium).Not.Nullable().Index("IDX_RecipeMetadata_NutritionLowSodium");
   //      Map(x => x.NutritionLowSugar).Not.Nullable().Index("IDX_RecipeMetadata_NutritionLowSugar");
   //      Map(x => x.NutritionTotalCalories).Not.Nullable().Index("IDX_RecipeMetadata_NutritionTotalCalories");
   //      Map(x => x.NutritionTotalCarbs).Not.Nullable().Index("IDX_RecipeMetadata_NutritionTotalCarbs");
   //      Map(x => x.NutritionTotalfat).Not.Nullable().Index("IDX_RecipeMetadata_NutritionTotalfat");
   //      Map(x => x.NutritionTotalSodium).Not.Nullable().Index("IDX_RecipeMetadata_NutritionTotalSodium");
   //      Map(x => x.NutritionTotalSugar).Not.Nullable().Index("IDX_RecipeMetadata_NutritionTotalSugar");
   //      Map(x => x.PhotoRes).Not.Nullable().Index("IDX_RecipeMetadata_PhotoRes");
   //      Map(x => x.SkillCommon).Not.Nullable().Index("IDX_RecipeMetadata_SkillCommon");
   //      Map(x => x.SkillEasy).Not.Nullable().Index("IDX_RecipeMetadata_SkillEasy");
   //      Map(x => x.SkillQuick).Not.Nullable().Index("IDX_RecipeMetadata_SkillQuick");
   //      Map(x => x.TasteMildToSpicy).Not.Nullable().Index("IDX_RecipeMetadata_TasteMildToSpicy");
   //      Map(x => x.TasteSavoryToSweet).Not.Nullable().Index("IDX_RecipeMetadata_TasteSavoryToSweet");
   //      Map(x => x.UsdaMatch).Not.Nullable();

   //      References(x => x.Recipe).Not.Nullable().Unique().Index("IDX_RecipeMetadata_RecipeId");
   //   }
   //}
}