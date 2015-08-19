namespace KitchenPC.Categorization.Models
{
    using System;

    using KitchenPC.Categorization.Interfaces;

    internal class IngredientCommonality : IIngredientCommonality
    {  
        public IngredientCommonality(Guid ingid, float commonality)
        {
            this.IngredientId = ingid;
            this.Commonality = commonality;
        }

        public Guid IngredientId { get; set; }

        public float Commonality { get; set; }
    }
}
