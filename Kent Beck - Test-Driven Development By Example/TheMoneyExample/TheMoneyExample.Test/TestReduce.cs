using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestReduce
    {
        [TestMethod]
        public void TestReduceSum()
        {
            var sum = new Sum(Money.Dollar(3), Money.Dollar(4));
            var bank = new Bank();
            var result = bank.Reduce(sum, "USD");

            Assert.AreEqual(Money.Dollar(7), result);
        }

        [TestMethod]
        public void TestReduceMoney()
        {
            var bank = new Bank();
            var result = bank.Reduce(Money.Dollar(1), "USD");

            Assert.AreEqual(Money.Dollar(1), result);
        }

        [TestMethod]
        public void ReduceMoney_DifferentCurrency()
        {
            // Arrange
            var bank = new Bank();
            bank.AddRate("CHF", "USD", 2);

            // Act
            var result = bank.Reduce(Money.Franc(2), "USD");

            // Assert
            Assert.AreEqual(Money.Dollar(1), result);
        }
    }
}
