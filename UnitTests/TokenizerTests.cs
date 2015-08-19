namespace KitchenPC.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;

    using KitchenPC.Categorization.Interfaces;
    using KitchenPC.Categorization.Logic;
    using KitchenPC.Categorization.Models.Tokens;

    using NUnit.Framework;

    [TestFixture]
    public class TokenizerTests
    {
        [Test]
        public void TokenizerTokenize_ShouldTokenizeAllExpectedProperties()
        {
            var recipe = Mock.Recipes.MIRACLE_DIET;
            var tokens = Tokenizer.Tokenize(recipe).ToList();

            Assert.AreEqual(4, tokens.Count(x => x is TextToken));
            Assert.AreEqual(1, tokens.Count(x => x is TimeToken));
            Assert.AreEqual(1, tokens.Count(x => x is IngredientToken));
        }

        [Test]
        public void TokenizerTokenize_ReturnsTokensWithCorrectValues()
        {
            var recipe = Mock.Recipes.MIRACLE_DIET;
            var tokens = Tokenizer.Tokenize(recipe).ToList();

            Assert.AreEqual("miracle", tokens[0].ToString());
            Assert.AreEqual("diet", tokens[1].ToString());
            Assert.AreEqual("literally", tokens[2].ToString());
            Assert.AreEqual("air", tokens[3].ToString());
            Assert.AreEqual("Long", tokens[4].ToString());
            Assert.AreEqual("[ING] - air", tokens[5].ToString());
        }
    }
}
