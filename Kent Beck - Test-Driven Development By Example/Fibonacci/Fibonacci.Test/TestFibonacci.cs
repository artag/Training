using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fibonacci.Test
{
    [TestClass]
    public class TestFibonacci
    {
        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(1, 1)]
        [DataRow(2, 1)]
        [DataRow(3, 2)]
        [DataRow(4, 3)]
        [DataRow(8, 21)]
        public void Fibonacci_GetNumberRecursive(int index, long expectedNumber)
        {
            var actualNumber = Fibonacci.GetNumberRecursive(index);
            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(1, 1)]
        [DataRow(2, 1)]
        [DataRow(3, 2)]
        [DataRow(4, 3)]
        [DataRow(8, 21)]
        public void Fibonacci_GetNumberIterative(int index, long expectedNumber)
        {
            var actualNumber = Fibonacci.GetNumberIterative(index);
            Assert.AreEqual(expectedNumber, actualNumber);
        }
    }
}
