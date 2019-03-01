using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestEquality
    {
        [TestMethod]
        public void FiveDollars_Equals_FiveDollars()
        {
            // Arrange
            var five1 = new Dollar(5);
            var five2 = new Dollar(5);

            // Assert
            Assert.IsTrue(five1.Equals(five2));
        }

        [TestMethod]
        public void FiveDollars_NotEquals_SixDollars()
        {
            // Arrange
            var five = new Dollar(5);
            var six = new Dollar(6);

            // Assert
            Assert.IsFalse(five.Equals(six));

        }

        [TestMethod]
        public void FiveFrancs_Equals_FiveFrancs()
        {
            // Arrange
            var five1 = new Franc(5);
            var five2 = new Franc(5);

            // Assert
            Assert.IsTrue(five1.Equals(five2));
        }

        [TestMethod]
        public void FiveFrancs_NotEquals_SixFrancs()
        {
            // Arrange
            var five = new Franc(5);
            var six = new Franc(6);

            // Assert
            Assert.IsFalse(five.Equals(six));

        }
    }
}
