namespace KitchenPC.UnitTests
{
    using NUnit.Core;
    using NUnit.Framework;

    [TestFixture]
    public class WeightTests
    {
        private static Weight testWeight;
        
        [Test]
        public void TestEmptyConstructor()
        {
            var newWeight = new Weight();
            Assert.AreEqual(0, newWeight.Value, "Empty constructor of Weight class does not work properly !");
        }

        [Test]
        public void TestConstructorWithParams()
        {
            var newWeight = new Weight(7);
            Assert.AreEqual(7, newWeight.Value, "Constructor with param(grams) of Weight class does not work properly !");
        }

        [Test]
        public void TestIntegerImplicitOperator()
        {
            var newWeight = new Weight(7);
            int integeredWeight = newWeight;
            Assert.AreEqual(7, integeredWeight, "Implicit operator \"int\"  of Weight class does not work properly !");
        }

        [Test]
        public void TestEquationOperator()
        {
            var firstWeight = new Weight(7);
            var secondWeight = new Weight(5);
            var thirdWeight = new Weight(7);

            Assert.IsFalse(firstWeight == secondWeight, "Operator for equation of Weight class does not work properly !");
            Assert.IsTrue(firstWeight == thirdWeight, "Operator for equation of Weight class does not work properly !");
            Assert.IsFalse(null == thirdWeight, "Operator for equation of Weight class does not work properly !");
        }

        [Test]
        public void TestInequalityOperator()
        {
            var firstWeight = new Weight(7);
            var secondWeight = new Weight(5);
            var thirdWeight = new Weight(7);

            Assert.IsTrue(firstWeight != secondWeight, "Operator for inequality of Weight class does not work properly !");
            Assert.IsFalse(firstWeight != thirdWeight, "Operator for inequality of Weight class does not work properly !");
        }

        [Test]
        public void TestCompareToMethodWithWeightParam()
        {
            var firstWeight = new Weight(36);
            var secondWeight = new Weight(25);
            var thirdWeight = new Weight(77);
            var fourthWeight = new Weight(36);

            Assert.AreEqual(1, firstWeight.CompareTo(secondWeight), "CompareTo method (with Weight param) of Weight class does not work properly");
            Assert.AreEqual(-1, firstWeight.CompareTo(thirdWeight), "CompareTo method (with Weight param) of Weight class does not work properly");
            Assert.AreEqual(0, firstWeight.CompareTo(fourthWeight), "CompareTo method (with Weight param) of Weight class does not work properly");
        }

        [Test]
        public void TestCompareToMethodWithIntParam()
        {
            var firstWeight = new Weight(36);
            var secondWeight = 25;
            var thirdWeight = 77;
            var fourthWeight = 36;

            Assert.AreEqual(1, firstWeight.CompareTo(secondWeight), "CompareTo method (with integer param) of Weight class does not work properly");
            Assert.AreEqual(-1, firstWeight.CompareTo(thirdWeight), "CompareTo method (with integer param) of Weight class does not work properly");
            Assert.AreEqual(0, firstWeight.CompareTo(fourthWeight), "CompareTo method (with integer param) of Weight class does not work properly");
        }

        [Test]
        public void TestEqualMethodWithWeightParam()
        {
            var firstWeight = new Weight(7);
            var secondWeight = new Weight(5);
            var thirdWeight = new Weight(7);

            Assert.IsFalse(firstWeight.Equals(secondWeight), "Equal method (with Weight param) of Weight class does not work properly !");
            Assert.IsTrue(firstWeight.Equals(thirdWeight), "Equal method (with Weight param) of Weight class does not work properly !");
            Assert.IsFalse(firstWeight.Equals(null), "Equal method (with Weight param) of Weight class does not work properly !");
        }

        [Test]
        public void TestEqualMethodWithIntegerParam()
        {
            var firstWeight = new Weight(7);
            var secondWeight = 5;
            var thirdWeight = 7;

            Assert.IsFalse(firstWeight.Equals(secondWeight), "Equal method (with integer param) of Weight class does not work properly !");
            Assert.IsTrue(firstWeight.Equals(thirdWeight), "Equal method (with integer param) of Weight class does not work properly !");
        }

        [Test]
        public void TestToStringMethod()
        {
            var firstWeight = new Weight(77);
            var firstWeightString = "77.00 g.";

            var secondWeight = new Weight(36);
            var secondWeightString = "36.00 g.";

            var thirdWeight = new Weight(9999);
            var thirdWeightString = "9999.00 g.";

            Assert.AreEqual(firstWeightString, firstWeight.ToString(), "ToString method of Weight class does not work properly !");
            Assert.AreEqual(secondWeightString, secondWeight.ToString(), "ToString method of Weight class does not work properly !");
            Assert.AreEqual(thirdWeightString, thirdWeight.ToString(), "ToString method of Weight class does not work properly !");
        }
    }
}
