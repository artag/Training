using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestExpressions
    {
        [TestMethod]
        public void TestSumPlusMoney()
        {
            // Arrange
            var fiveDollars = Money.Dollar(5);
            var tenFrancs = Money.Franc(10);

            var bank = new Bank();
            bank.AddRate("CHF", "USD", 2);

            // Act
            var sum = new Sum(fiveDollars, tenFrancs).Plus(fiveDollars);
            var result = bank.Reduce(sum, "USD");

            // Assert
            Assert.AreEqual(Money.Dollar(15), result);
        }

        [TestMethod]
        public void TestSumTimes()
        {
            // Arrange
            var fiveDollars = Money.Dollar(5);
            var tenFrancs = Money.Franc(10);

            var bank = new Bank();
            bank.AddRate("CHF", "USD", 2);

            // Act
            var sum = new Sum(fiveDollars, tenFrancs).Times(2);
            var result = bank.Reduce(sum, "USD");

            Assert.AreEqual(Money.Dollar(20), result);
        }
    }
}
