using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMoneyExample.Test
{
    [TestClass]
    public class TestAddition
    {
        [TestMethod]
        [DataRow(5, 5, 10)]
        [DataRow(5, 6, 11)]
        public void Dollar_TestAddition(int augend, int addend, int sum)
        {
            var actualSum = Money.Dollar(augend).Plus(Money.Dollar(addend));
            Assert.AreEqual(Money.Dollar(sum), actualSum);
        }

        [TestMethod]
        public void SimpleAdditionWithReduce()
        {
            var five = Money.Dollar(5);
            IExpression sum = five.Plus(five);
            var bank = new Bank();
            var reduced = bank.Reduce(sum, "USD");
            Assert.AreEqual(Money.Dollar(10), reduced);
        }
    }
}
