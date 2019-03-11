using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void CreateFiveDollars_CheckAmount()
        {
            var fiveDollars = MoneyProvider.Dollar(5);
            Assert.AreEqual(5, fiveDollars.Amount);
        }

        [TestMethod]
        public void CreateFiveDollars_CheckCurrency()
        {
            var fiveDollars = MoneyProvider.Dollar(5);
            Assert.AreEqual(Currency.USD, fiveDollars.Currency);
        }

        [TestMethod]
        public void CreateFranc_CheckAmount()
        {
            var fourFrancs = MoneyProvider.Franc(4);
            Assert.AreEqual(4, fourFrancs.Amount);
        }

        [TestMethod]
        public void CreateFranc_CheckCurrency()
        {
            var fourFrancs = MoneyProvider.Franc(4);
            Assert.AreEqual(Currency.CHF, fourFrancs.Currency);
        }
    }
}
