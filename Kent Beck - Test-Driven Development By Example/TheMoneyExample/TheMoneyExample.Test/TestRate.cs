using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestRate
    {
        [TestMethod]
        public void IdentiryRate()
        {
            var bank = new Bank();
            Assert.AreEqual(1, bank.Rate("USD", "USD"));
        }
    }
}
