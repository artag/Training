using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestEquality
    {
        [TestMethod]
        public void FiveDollars_FiveDollars_AreEqual()
        {
            var fiveDollars1 = MoneyProvider.Dollar(5);
            var fiveDollars2 = MoneyProvider.Dollar(5);
            Assert.AreEqual(fiveDollars1, fiveDollars2);
        }

        [TestMethod]
        public void FiveDollars_SixDollars_AreNotEqual()
        {
            var fiveDollars = MoneyProvider.Dollar(5);
            var sixDollars = MoneyProvider.Dollar(6);
            Assert.AreNotEqual(fiveDollars, sixDollars);
        }

        [TestMethod]
        public void TwoFrancs_TwoFrancs_AreEqual()
        {
            var twoFrancs1 = MoneyProvider.Franc(2);
            var twoFrancs2 = MoneyProvider.Franc(2);
            Assert.AreEqual(twoFrancs1, twoFrancs2);
        }

        [TestMethod]
        public void NineFrancs_TenFrancs_AreNotEqual()
        {
            var nineFrancs = MoneyProvider.Franc(9);
            var tenFrancs = MoneyProvider.Franc(10);
            Assert.AreNotEqual(nineFrancs, tenFrancs);
        }

        [TestMethod]
        public void FiveDollars_FiveFrancs_AreNotEqual()
        {
            var fiveDollars = MoneyProvider.Dollar(5);
            var fiveFrancs = MoneyProvider.Franc(5);
            Assert.AreNotEqual(fiveDollars, fiveFrancs);
        }
    }
}
