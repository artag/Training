using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestAddition
    {
        [TestMethod]
        public void SimpleAdditionWithReduce()
        {
            // Arrange
            var five = Money.Dollar(5);
            var bank = new Bank();

            // Act
            var sum = five.Plus(five);
            var reduced = bank.Reduce(sum, "USD");

            // Assert
            Assert.AreEqual(Money.Dollar(10), reduced);
        }

        [TestMethod]
        public void Money_MethodPlus_ReturnsSum()
        {
            // Act
            var five = Money.Dollar(5);
            var result = five.Plus(five);
            var sum = (Sum)result;

            // Assert
            Assert.AreEqual(five, sum.Augend);
            Assert.AreEqual(five, sum.Addend);
        }

        [TestMethod]
        public void FiveDollars_Plus_TenFrancs_EqualsTenDollars()
        {
            // Arrange
            var fiveDollars = Money.Dollar(5);
            var tenFrancs = Money.Franc(10);
            var bank = new Bank();
            bank.AddRate("CHF", "USD", 2);

            // Act
            var sum = fiveDollars.Plus(tenFrancs);
            var result = bank.Reduce(sum, "USD");

            // Assert
            Assert.AreEqual(Money.Dollar(10), result);
        }
    }
}
