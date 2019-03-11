using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestCurrencyPair
    {
        [TestMethod]
        public void PairUsdChf_PairUsdChf_AreEqual()
        {
            // Arrange
            var pair1 = new CurrencyPair(Currency.USD, Currency.CHF);
            var pair2 = new CurrencyPair(Currency.USD, Currency.CHF);

            // Assert
            Assert.AreEqual(pair1, pair2);
        }

        [TestMethod]
        public void PairChfUsd_PairUsdChf_AreNotEqual()
        {
            // Arrange
            var pair1 = new CurrencyPair(Currency.USD, Currency.CHF);
            var pair2 = new CurrencyPair(Currency.CHF, Currency.USD);

            // Assert
            Assert.AreNotEqual(pair1, pair2);
        }
    }
}
