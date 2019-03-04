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
            var dollars = Money.Dollar(amount);

            // Assert
            Assert.AreEqual(Money.Dollar(product), dollars.Times(multiplier));
        }

        [TestMethod]
        public void Dollar_TestTwoMultiplications()
        {
            // Arrange
            var five = Money.Dollar(5);

            // Assert
            Assert.AreEqual(Money.Dollar(10), five.Times(2));
            Assert.AreEqual(Money.Dollar(15), five.Times(3));
        }

        [TestMethod]
        [DataRow(5, 2, 10)]
        [DataRow(5, 3, 15)]
        [DataRow(2, 3, 6)]
        public void Franc_TestMultiplication(int amount, int multiplier, int product)
        {
            // Arrange
            var francs = Money.Franc(amount);

            // Assert
            Assert.AreEqual(Money.Franc(product), francs.Times(multiplier));
        }

        [TestMethod]
        public void Franc_TestTwoMultiplications()
        {
            // Arrange
            var five = Money.Franc(5);

            // Assert
            Assert.AreEqual(Money.Franc(10), five.Times(2));
            Assert.AreEqual(Money.Franc(15), five.Times(3));
        }
    }
}
