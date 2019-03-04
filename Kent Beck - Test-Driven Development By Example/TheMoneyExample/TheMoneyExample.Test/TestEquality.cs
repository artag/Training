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
            var five1 = Money.Dollar(5);
            var five2 = Money.Dollar(5);

            // Assert
            Assert.IsTrue(five1.Equals(five2));
        }

        [TestMethod]
        public void FiveDollars_NotEquals_SixDollars()
        {
            // Arrange
            var five = Money.Dollar(5);
            var six = Money.Dollar(6);

            // Assert
            Assert.IsFalse(five.Equals(six));

        }

        [TestMethod]
        public void FiveFrancs_Equals_FiveFrancs()
        {
            // Arrange
            var five1 = Money.Franc(5);
            var five2 = Money.Franc(5);

            // Assert
            Assert.IsTrue(five1.Equals(five2));
        }

        [TestMethod]
        public void FiveFrancs_NotEquals_SixFrancs()
        {
            // Arrange
            var five = Money.Franc(5);
            var six = Money.Franc(6);

            // Assert
            Assert.IsFalse(five.Equals(six));

        }

        [TestMethod]
        public void FiveFrancs_NotEquals_FiveDollars()
        {
            // Arrange
            var fiveFrancs = Money.Franc(5);
            var fiveDollars = Money.Dollar(5);

            // Assert
            Assert.IsFalse(fiveFrancs.Equals(fiveDollars));
        }
    }
}
