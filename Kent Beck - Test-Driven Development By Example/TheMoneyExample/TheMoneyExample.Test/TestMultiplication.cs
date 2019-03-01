using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestMultiplication
    {
        [TestMethod]
        [DataRow(5, 2, 10)]
        [DataRow(5, 3, 15)]
        [DataRow(2, 3, 6)]
        public void Dollar_TestMultiplication(int amount, int multiplier, int product)
        {
            // Arrange
            var dollars = new Dollar(amount);

            // Assert
            Assert.AreEqual(new Dollar(product), dollars.Times(multiplier));
        }

        [TestMethod]
        public void Dollar_TestTwoMultiplications()
        {
            // Arrange
            var five = new Dollar(5);

            // Assert
            Assert.AreEqual(new Dollar(10), five.Times(2));
            Assert.AreEqual(new Dollar(15), five.Times(3));
        }

        [TestMethod]
        [DataRow(5, 2, 10)]
        [DataRow(5, 3, 15)]
        [DataRow(2, 3, 6)]
        public void Franc_TestMultiplication(int amount, int multiplier, int product)
        {
            // Arrange
            var francs = new Franc(amount);

            // Assert
            Assert.AreEqual(new Franc(product), francs.Times(multiplier));
        }

        [TestMethod]
        public void Franc_TestTwoMultiplications()
        {
            // Arrange
            var five = new Franc(5);

            // Assert
            Assert.AreEqual(new Franc(10), five.Times(2));
            Assert.AreEqual(new Franc(15), five.Times(3));
        }
    }
}
