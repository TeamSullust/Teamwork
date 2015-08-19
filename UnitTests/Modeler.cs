namespace KitchenPC.UnitTests
{
    using System;
    using System.Diagnostics;
    using KitchenPC.Context;
    using KPCServer.UnitTests;
    using NUnit.Framework;

    [TestFixture]
    internal class ModelerTests
    {
        public static Guid ingEggs = new Guid("948aeda5-ffff-41bd-af4e-71d1c740db76");

        public static Guid ingMilk = new Guid("5a698842-54a9-4ed2-b6c3-aea1bcd157cd");

        public static Guid ingFlour = new Guid("daa8fbf6-3347-41b9-826d-078cd321402e");

        public static Guid ingCheese = new Guid("5ee315ea-7ef6-4fa5-809a-dc9931a01ed1");

        public static Guid ingChicken = new Guid("55344339-8d1d-4892-a117-ec8018a5e483");

        public static Guid ingChineseChestnuts = new Guid("b124f851-5a10-4432-9455-00d3471ab802");

        public static Guid ingGreenTurtle = new Guid("d81e16da-2de9-4184-92f5-066e2fab0b71");

        private IKPCContext context;

        [TestFixtureSetUp]
        public void Setup()
        {
            Trace.Write("Creating DB Snapshot from XML file... ");
            this.context = new MockContext();
            this.context.Initialize();
            Trace.WriteLine("Done!");
        }

        [Test]
        public void TestNoRatingModeler()
        {
            Trace.WriteLine("Running NoRating Test.");
            var profile = new MockNoRatingsUserProfile();
            var session = this.context.CreateModelingSession(profile);

            Trace.WriteLine("Running NoRatingModel Test (Efficient)");
            var efficientSet = session.Generate(5, 1); // Test for most efficient set
            Assert.AreEqual(5, efficientSet.RecipeIds.Length);

            Trace.WriteLine("Running NoRatingModel Test (Balanced)");
            var balancedSet = session.Generate(5, 3); // Test for balanced model
            Assert.AreEqual(5, balancedSet.RecipeIds.Length);

            Trace.WriteLine("Running NoRatingModel Test (Recommended)");
            var ratedSet = session.Generate(5, 5); // Test for recipes user most likely to rate highly (basically get suggestions, ignore pantry)
            Assert.AreEqual(5, ratedSet.RecipeIds.Length);
        }

        [Test]
        [ExpectedException(typeof(ImpossibleQueryException))]
        public void TestImpossibleFilterModeler()
        {
            Trace.WriteLine("Running ImpossibleFilter Test.");
            var profile = new MockImpossibleFilterUserProfile(); // Only No Pork recipes are allowed, of which there are none in our mock data
            var session = this.context.CreateModelingSession(profile);
            var set = session.Generate(5, 1);
        }

        [Test]
        [ExpectedException(typeof(ImpossibleQueryException))]
        public void TestImpossiblePantryModeler()
        {
            Trace.WriteLine("Running ImpossiblePantry Test.");

            // Generates a user with pantry ingredients that aren't used by any recipe thus the pantry is considered empty and throws and impossible query exception
            var profile = new MockImpossiblePantryUserProfile();
            var session = this.context.CreateModelingSession(profile);
            var set = session.Generate(5, 1);
        }

        [Test]
        public void TestNormalModeler()
        {
            var profile = new MockNormalUserProfile();
            var session = this.context.CreateModelingSession(profile);

            Trace.WriteLine("Running NormalModel Test (Efficient)");
            var efficientSet = session.Generate(5, 1); // Test for most efficient set
            Assert.AreEqual(5, efficientSet.RecipeIds.Length);

            Trace.WriteLine("Running NormalModel Test (Balanced)");
            var balancedSet = session.Generate(5, 3); // Test for balanced model
            Assert.AreEqual(5, balancedSet.RecipeIds.Length);

            Trace.WriteLine("Running NormalModel Test (Recommended)");
            var ratedSet = session.Generate(5, 5); // Test for recipes user most likely to rate highly (basically get suggestions, ignore pantry)
            Assert.AreEqual(5, ratedSet.RecipeIds.Length);

            // - Test profile with specific AllowedTags, and AllowedTags == null
        }
    }
}