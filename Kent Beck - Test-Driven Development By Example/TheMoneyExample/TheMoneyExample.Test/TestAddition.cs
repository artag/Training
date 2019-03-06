using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestAddition
    {
        [TestMethod]
        public void SimpleAdditionWithReduce()
        {
            var five = Money.Dollar(5);
            var sum = five.Plus(five);
            var bank = new Bank();
            var reduced = bank.Reduce(sum, "USD");

            Assert.AreEqual(Money.Dollar(10), reduced);
        }

        [TestMethod]
        public void Money_MethodPlus_ReturnsSum()
        {
            var five = Money.Dollar(5);
            var result = five.Plus(five);
            var sum = (Sum)result;

            Assert.AreEqual(five, sum.Augend);
            Assert.AreEqual(five, sum.Addend);
        }

        [TestMethod]
        public void TestReduceSum()
        {
            var sum = new Sum(Money.Dollar(3), Money.Dollar(4));
            var bank = new Bank();
            var result = bank.Reduce(sum, "USD");

            Assert.AreEqual(Money.Dollar(7), result);
        }

        [TestMethod]
        public void TestReduceMoney()
        {
            var bank = new Bank();
            var result = bank.Reduce(Money.Dollar(1), "USD");

            Assert.AreEqual(Money.Dollar(1), result);
        }
    }
}
