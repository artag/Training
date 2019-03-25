using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gcd.Test
{
    [TestClass]
    public class TestGcd
    {
        [TestMethod]
        [DataRow(1680, 640, 80)]
        [DataRow(640, 1680, 80)]
        public void TestRecursive1(int width, int height, int expected)
        {
            Assert.AreEqual(expected, CommonDivisor.FindRecursiveWay(width, height));
        }

        [TestMethod]
        [DataRow(1071, 462, 21)]
        [DataRow(462, 1071, 21)]
        public void TestRecursive2(int width, int height, int expected)
        {
            Assert.AreEqual(expected, CommonDivisor.FindRecursiveWay(width, height));
        }

        [TestMethod]
        [DataRow(13, 2, 1)]
        [DataRow(2, 13, 1)]
        public void TestRecursive3(int width, int height, int expected)
        {
            Assert.AreEqual(expected, CommonDivisor.FindRecursiveWay(width, height));
        }

        [TestMethod]
        [DataRow(7, 7, 7)]
        public void TestRecursive4(int width, int height, int expected)
        {
            Assert.AreEqual(expected, CommonDivisor.FindRecursiveWay(width, height));
        }

        [TestMethod]
        [DataRow(1680, 640, 80)]
        [DataRow(640, 1680, 80)]
        public void TestIterative1(int width, int height, int expected)
        {
            Assert.AreEqual(expected, CommonDivisor.FindIterativeWay(width, height));
        }

        [TestMethod]
        [DataRow(1071, 462, 21)]
        [DataRow(462, 1071, 21)]
        public void TestIterative2(int width, int height, int expected)
        {
            Assert.AreEqual(expected, CommonDivisor.FindIterativeWay(width, height));
        }

        [TestMethod]
        [DataRow(13, 2, 1)]
        [DataRow(2, 13, 1)]
        public void TestIterative3(int width, int height, int expected)
        {
            Assert.AreEqual(expected, CommonDivisor.FindIterativeWay(width, height));
        }

        [TestMethod]
        [DataRow(7, 7, 7)]
        public void TestIterative4(int width, int height, int expected)
        {
            Assert.AreEqual(expected, CommonDivisor.FindIterativeWay(width, height));
        }
    }
}
