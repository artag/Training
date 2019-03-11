using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestSum
    {
        private readonly Calculation _calculation;

        public TestSum()
        {
            var repository = new CurrencyExchangeRepository();
            var exchanger = new CurrencyExchanger(repository);
            _calculation = new Calculation(exchanger);

            repository.AddExchangeRate(Currency.USD, Currency.CHF, 2.0/1.0);
        }

        [TestMethod]
        public void ThreeDollars_Plus_FourDollars_Equals_SevenDollars()
        {
            // Arrange
            var threeDollars = MoneyProvider.Dollar(3);
            var fourDollars = MoneyProvider.Dollar(4);

            var sevenDollars = MoneyProvider.Dollar(7);

            // Act
            var sum = _calculation.Sum(threeDollars, fourDollars, Currency.USD);

            // Assert
            Assert.AreEqual(sevenDollars, sum);
        }

        [TestMethod]
        public void SixFrancs_Plus_TwoFrancs_Equals_EightFrancs()
        {
            // Arrange
            var sixFrancs = MoneyProvider.Franc(6);
            var twoFrancs = MoneyProvider.Franc(2);

            var eightFrancs = MoneyProvider.Franc(8);

            // Act
            var sum = _calculation.Sum(sixFrancs, twoFrancs, Currency.CHF);

            // Assert
            Assert.AreEqual(eightFrancs, sum);
        }

        [TestMethod]
        public void FiveDollars_Plus_TenFrancs_Equals_TenDollars()
        {
            // Arrange
            var fiveDollars = MoneyProvider.Dollar(5);
            var tenFrancs = MoneyProvider.Franc(10);

            var tenDollars = MoneyProvider.Dollar(10);

            // Act
            var sum = _calculation.Sum(fiveDollars, tenFrancs, Currency.USD);

            // Assert
            Assert.AreEqual(tenDollars, sum);
        }

        [TestMethod]
        public void FiveDollars_Plus_TenFrancs_Equals_TwentyFrancs()
        {
            // Arrange
            var fiveDollars = MoneyProvider.Dollar(5);
            var tenFrancs = MoneyProvider.Franc(10);

            var twentyFrancs = MoneyProvider.Franc(20);

            // Act
            var sum = _calculation.Sum(fiveDollars, tenFrancs, Currency.CHF);

            // Assert
            Assert.AreEqual(twentyFrancs, sum);
        }

        [TestMethod]
        public void SixFrancs_Plus_TwoDollars_Equals_TenFrancs()
        {
            // Arrange
            var sixFrancs = MoneyProvider.Franc(6);
            var twoDollars = MoneyProvider.Dollar(2);

            var tenFrancs = MoneyProvider.Franc(10);

            // Act
            var sum = _calculation.Sum(sixFrancs, twoDollars, Currency.CHF);

            // Assert
            Assert.AreEqual(tenFrancs, sum);
        }

        [TestMethod]
        public void SixFrancs_Plus_TwoDollars_Equals_FiveDollars()
        {
            // Arrange
            var sixFrancs = MoneyProvider.Franc(6);
            var twoDollars = MoneyProvider.Dollar(2);

            var fiveDollars = MoneyProvider.Dollar(5);

            // Act
            var sum = _calculation.Sum(sixFrancs, twoDollars, Currency.USD);

            // Assert
            Assert.AreEqual(fiveDollars, sum);
        }
    }
}
