using System;
using System.Collections.Generic;
using System.Linq;
using KitchenPC.Data.DTO;

namespace KitchenPC.Data
{
    /// <summary>Holds all data from a persisted KitchenPC context.  Used to migrate data from one context to another.</summary>
    public class DataStore
    {
        // Indexes
        private Dictionary<Guid, RecipeMetadata> indexedRecipeMetadata;

        private Dictionary<Guid, DTO.Ingredients> indexedIngredients;

        private Dictionary<Guid, IngredientMetadata> indexedIngredientMetadata;

        private Dictionary<Guid, IngredientForms> indexedIngredientForms;

        private Dictionary<Guid, DTO.Recipes> indexedRecipes;

        private Dictionary<Guid, RecipeIngredients[]> indexedRecipeIngredients;

        private IEnumerable<RecipeData> searchIndex;

        // Core Data (Immutable)
        public IngredientForms[] IngredientForms { get; set; }

        public IngredientMetadata[] IngredientMetadata { get; set; }

        public DTO.Ingredients[] Ingredients { get; set; }

        public NlpAnomalousIngredients[] NlpAnomalousIngredients { get; set; }

        public NlpDefaultPairings[] NlpDefaultPairings { get; set; }

        public NlpFormSynonyms[] NlpFormSynonyms { get; set; }

        public NlpIngredientSynonyms[] NlpIngredientSynonyms { get; set; }

        public NlpPrepNotes[] NlpPrepNotes { get; set; }

        public NlpUnitSynonyms[] NlpUnitSynonyms { get; set; }

        // Recipe Data
        public List<DTO.Recipes> Recipes { get; set; }

        public List<RecipeMetadata> RecipeMetadata { get; set; }

        public List<RecipeIngredients> RecipeIngredients { get; set; }

        // User Data
        public List<Favorites> Favorites { get; set; }

        public List<DTO.Menus> Menus { get; set; }

        public List<QueuedRecipes> QueuedRecipes { get; set; }

        public List<RecipeRatings> RecipeRatings { get; set; }

        public List<DTO.ShoppingLists> ShoppingLists { get; set; }

        public List<ShoppingListItems> ShoppingListItems { get; set; }

        public void ClearIndexes()
        {
            indexedRecipeMetadata = null;
            indexedIngredients = null;
            indexedIngredientMetadata = null;
            indexedIngredientForms = null;
            indexedRecipes = null;
            indexedRecipeIngredients = null;
            searchIndex = null;
        }

        public Dictionary<Guid, RecipeMetadata> GetIndexedRecipeMetadata()
        {
            return this.indexedRecipeMetadata
                   ?? (this.indexedRecipeMetadata = this.RecipeMetadata.ToDictionary(m => m.RecipeId));
        }

        public Dictionary<Guid, DTO.Ingredients> GetIndexedIngredients()
        {
            return this.indexedIngredients
                   ?? (this.indexedIngredients = this.Ingredients.ToDictionary(i => i.IngredientId));
        }

        public Dictionary<Guid, IngredientMetadata> GetIndexedIngredientMetadata()
        {
            return this.indexedIngredientMetadata
                   ?? (this.indexedIngredientMetadata =
                       this.IngredientMetadata.ToDictionary(i => i.IngredientMetadataId));
        }

        public Dictionary<Guid, IngredientForms> GetIndexedIngredientForms()
        {
            return this.indexedIngredientForms
                   ?? (this.indexedIngredientForms = this.IngredientForms.ToDictionary(i => i.IngredientFormId));
        }

        public Dictionary<Guid, DTO.Recipes> GetIndexedRecipes()
        {
            return this.indexedRecipes ?? (this.indexedRecipes = this.Recipes.ToDictionary(r => r.RecipeId));
        }

        public Dictionary<Guid, RecipeIngredients[]> GetIndexedRecipeIngredients()
        {
            return this.indexedRecipeIngredients
                   ?? (this.indexedRecipeIngredients =
                       this.RecipeIngredients.GroupBy(r => r.RecipeId).ToDictionary(g => g.Key, i => i.ToArray()));
        }

        public IEnumerable<RecipeData> GetSearchIndex()
        {
            var indexIngredients = GetIndexedRecipeIngredients();
            var indexMetadata = GetIndexedRecipeMetadata();

            return this.searchIndex
                   ?? (this.searchIndex =
                       this.Recipes.Select(
                           v =>
                           new RecipeData
                               {
                                   Recipe = v,
                                   Metadata =
                                       indexMetadata.ContainsKey(v.RecipeId) ? indexMetadata[v.RecipeId] : null,
                                   Ingredients =
                                       indexIngredients.ContainsKey(v.RecipeId)
                                           ? indexIngredients[v.RecipeId]
                                           : null
                               }));
        }
    }
}