using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestCurrency
    {
        [TestMethod]
        public void GetDollarCurrency()
        {
            // Assert
            Assert.AreEqual("USD", Money.Dollar(1).Currency);
        }

        [TestMethod]
        public void GetFrancCurrency()
        {
            // Assert
            Assert.AreEqual("CHF", Money.Franc(1).Currency);
        }
    }
}
