namespace KitchenPC.Categorization.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using KitchenPC.Categorization.Interfaces;
    using KitchenPC.Categorization.Models.Tokens;
    using KitchenPC.Recipes;

    // Takes a Recipe object and returns an enumeration of Token objects
    public static class Tokenizer
    {
        private static readonly Regex Valid = new Regex(@"[a-z]", RegexOptions.IgnoreCase); // All tokens have to have at least one letter in them

        public static IEnumerable<IToken> ParseText(string text)
        {
            var parts = Regex.Split(text, @"[^a-z0-9\'\$\-]", RegexOptions.IgnoreCase);
            return parts.Where(p => Valid.IsMatch(p)).Select(p => new TextToken(p));
        }

        // What should IToken have? Should Token be a Generic Item<T>?
        public static IEnumerable<IToken> Tokenize(Recipe recipe)
        {
            var tokens = new List<IToken>();
            tokens.AddRange(ParseText(recipe.Title ?? string.Empty));
            tokens.AddRange(ParseText(recipe.Description ?? string.Empty));
            tokens.AddRange(ParseText(recipe.Method ?? string.Empty));
            tokens.Add(new TimeToken(recipe.CookTime + recipe.PrepTime));
            tokens.AddRange(
                recipe.Ingredients.NeverNull()
                    .Select(ingredientUsage => new IngredientToken(ingredientUsage.Ingredient)));

            return tokens;
        }
    }
}