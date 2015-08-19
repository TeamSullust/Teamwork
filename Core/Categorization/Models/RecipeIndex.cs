namespace KitchenPC.Categorization.Models
{
    using System.Collections.Generic;
    using System.Linq;

    using KitchenPC.Categorization.Interfaces;
    using KitchenPC.Categorization.Logic;
    using KitchenPC.Categorization.Models.Tokens;
    using KitchenPC.Recipes;

    public class RecipeIndex
    {
        private readonly Dictionary<IToken, int> index = new Dictionary<IToken, int>();

        public int EntryCount
        {
            get
            {
                return this.index.Values.Sum();
            }
        }

        public int GetTokenCount(IToken token)
        {
            return this.index.ContainsKey(token) ? this.index[token] : 0;
        }

        public void Add(Recipe recipe)
        {
            var tokens = Tokenizer.Tokenize(recipe);
            foreach (var token in tokens)
            {
                if (this.index.ContainsKey(token))
                {
                    this.index[token]++;
                }
                else
                {
                    this.index.Add(token, 1);
                }
            }
        }
    }
}