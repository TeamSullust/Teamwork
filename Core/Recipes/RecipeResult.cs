namespace KitchenPC.Recipes
{
    using System;

   public class RecipeResult
   {
       public bool RecipeCreated { get; set; }

       public bool RecipeUpdated { get; set; }

       public Guid? NewRecipeId { get; set; }
   }
}