using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestPair
    {
        [TestMethod]
        public void TwoPairs_AreEquals()
        {
            Assert.AreEqual(new Pair("USD", "CHF"), new Pair("USD", "CHF"));
        }
    }
}
