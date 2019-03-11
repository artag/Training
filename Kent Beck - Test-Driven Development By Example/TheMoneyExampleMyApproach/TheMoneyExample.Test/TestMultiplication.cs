using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestMultiplication
    {
        private readonly Calculation _calculation;

        public TestMultiplication()
        {
            var repository = new CurrencyExchangeRepository();
            var exchanger = new CurrencyExchanger(repository);
            _calculation = new Calculation(exchanger);

            repository.AddExchangeRate(Currency.USD, Currency.CHF, 2.0/1.0);
        }

        [TestMethod]
        public void TwoDollars_Times_Three_Equals_SixDollars()
        {
            // Arrange
            var twoDollars = MoneyProvider.Dollar(2);
            var sixDollars = MoneyProvider.Dollar(6);

            // Act
            var product = _calculation.Product(twoDollars, 3, Currency.USD);

            // Assert
            Assert.AreEqual(sixDollars, product);
        }

        [TestMethod]
        public void TwoDollars_Times_Three_Equals_TwelveFrancs()
        {
            // Arrange
            var twoDollars = MoneyProvider.Dollar(2);
            var twelveFrancs = MoneyProvider.Franc(12);

            // Act
            var product = _calculation.Product(twoDollars, 3, Currency.CHF);

            // Assert
            Assert.AreEqual(twelveFrancs, product);
        }

        [TestMethod]
        public void ThreeFrancs_Times_Four_Equals_TwelveFrancs()
        {
            // Arrange
            var threeFrancs = MoneyProvider.Franc(3);
            var twelveFrancs = MoneyProvider.Franc(12);

            // Act
            var product = _calculation.Product(threeFrancs, 4, Currency.CHF);

            // Assert
            Assert.AreEqual(twelveFrancs, product);
        }

        [TestMethod]
        public void ThreeFrancs_Times_Four_Equals_SixDollars()
        {
            // Arrange
            var threeFrancs = MoneyProvider.Franc(3);
            var sixDollars = MoneyProvider.Dollar(6);

            // Act
            var product = _calculation.Product(threeFrancs, 4, Currency.USD);

            // Assert
            Assert.AreEqual(sixDollars, product);
        }
    }
}
