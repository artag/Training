using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestMultiplication
    {
        [TestMethod]
        public void FiveTimesTwo_Equals_Ten()
        {
            // Arrange
            var five = new Dollar(5);

            // Assert
            Assert.AreEqual(new Dollar(10), five.Times(2));
        }

        [TestMethod]
        public void FiveTimesThree_Equals_Fifteen()
        {
            // Arrange
            var five = new Dollar(5);

            // Assert
            Assert.AreEqual(new Dollar(15), five.Times(3));
        }

        [TestMethod]
        public void TwoTimesThree_Equals_Six()
        {
            // Arrange
            var two = new Dollar(2);

            // Assert
            Assert.AreEqual(new Dollar(6), two.Times(3));
        }

        [TestMethod]
        public void TestTwoMultiplications()
        {
            // Arrange
            var five = new Dollar(5);

            // Assert
            Assert.AreEqual(new Dollar(10), five.Times(2));
            Assert.AreEqual(new Dollar(15), five.Times(3));
        }
    }
}
