using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestCurrencyExchangeRepository
    {
        [TestMethod]
        public void AddExchangeRate_USDtoCHF()
        {
            var repository = new CurrencyExchangeRepository();
            repository.AddExchangeRate(Currency.USD, Currency.CHF, 2.0/1.0);

            var pair = new CurrencyPair(Currency.USD, Currency.CHF);
            Assert.IsTrue(Math.Abs(2.0/1.0 - repository.Rates[pair]) < double.Epsilon);
        }

        [TestMethod]
        public void AddExchangeRate_USDtoCHF_GetExchangeRate_USDtoCHF()
        {
            var repository = new CurrencyExchangeRepository();
            repository.AddExchangeRate(Currency.USD, Currency.CHF, 2.0/1.0);

            var rate = repository.GetExchangeRate(Currency.USD, Currency.CHF);

            Assert.IsTrue(Math.Abs(2.0/1.0 - rate) < double.Epsilon);
        }

        [TestMethod]
        public void AddExchangeRate_CHFtoUSD_GetExchangeRate_CHFtoUSD()
        {
            var repository = new CurrencyExchangeRepository();
            repository.AddExchangeRate(Currency.USD, Currency.CHF, 2.0/1.0);

            var rate = repository.GetExchangeRate(Currency.CHF, Currency.USD);

            Assert.IsTrue(Math.Abs(1.0/2.0 - rate) < double.Epsilon);
        }
    }
}
