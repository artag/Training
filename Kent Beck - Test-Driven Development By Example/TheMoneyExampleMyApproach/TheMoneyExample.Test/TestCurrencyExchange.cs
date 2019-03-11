using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestCurrencyExchange
    {
        private readonly CurrencyExchangeRepository _repository;
        private readonly CurrencyExchanger _exchanger;

        public TestCurrencyExchange()
        {
            _repository = new CurrencyExchangeRepository();
            _repository.AddExchangeRate(Currency.USD, Currency.CHF, 2.0/1.0);

            _exchanger = new CurrencyExchanger(_repository);
        }

        [TestMethod]
        public void Exchange_FiveDollars_To_Dollars_Result_FiveDollars()
        {
            // Arrange
            var fiveDollars = MoneyProvider.Dollar(5);

            // Act
            var result = _exchanger.Exchange(fiveDollars, Currency.USD);

            // Assert
            Assert.AreEqual(fiveDollars, result);
        }

        [TestMethod]
        public void Exchange_FiveFrancs_To_Francs_Result_FiveFrancs()
        {
            // Arrange
            var fiveFrancs = MoneyProvider.Franc(5);

            // Act
            var result = _exchanger.Exchange(fiveFrancs, Currency.CHF);

            // Assert
            Assert.AreEqual(fiveFrancs, result);
        }

        [TestMethod]
        public void Exchange_TwoFrancs_To_Dollars_Result_OneDollar()
        {
            // Arrange
            var twoFrancs = MoneyProvider.Franc(2);
            var oneDollar = MoneyProvider.Dollar(1);

            // Act
            var result = _exchanger.Exchange(twoFrancs, Currency.USD);

            // Assert
            Assert.AreEqual(oneDollar, result);
        }

        [TestMethod]
        public void Exchange_ThreeDollars_To_Francs_Result_SixFrancs()
        {
            // Arrange
            var dollars = MoneyProvider.Dollar(3);
            var franc = MoneyProvider.Franc(6);

            // Act
            var result = _exchanger.Exchange(dollars, Currency.CHF);

            // Assert
            Assert.AreEqual(franc, result);
        }
    }
}
