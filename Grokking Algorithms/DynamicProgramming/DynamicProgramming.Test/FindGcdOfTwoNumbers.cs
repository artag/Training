using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicProgramming.Test
{
    [TestClass]
    public class FindGcdOfTwoNumbers
    {
        [TestMethod]
        [DataRow(48, 64, 16)]
        [DataRow(64, 48, 16)]
        public void Gcd_For_48_And_64_Is_16(double a, double b, double expected)
        {
            var actual = GreatestCommonDivisor.Find(a, b);
            Assert.IsTrue(Math.Abs(actual - expected) < double.Epsilon);
        }

        [TestMethod]
        [DataRow(48, 48, 48)]
        [DataRow(64, 64, 64)]
        [DataRow(0.9, 0.9, 0.9)]
        [DataRow(1.2, 1.2, 1.2)]
        public void Gcd_For_Equal_Numbers_Is_Number(double a, double b, double expected)
        {
            var actual = GreatestCommonDivisor.Find(a, b);
            Assert.IsTrue(Math.Abs(actual - expected) < double.Epsilon);
        }

        [TestMethod]
        [DataRow(0.9, 1.2, 0.3)]
        [DataRow(1.2, 0.9, 0.3)]
        public void Gcd_For_0p9_And_1p2_Is_0p3(double a, double b, double expected)
        {
            var actual = GreatestCommonDivisor.Find(a, b);
            Assert.IsTrue(Math.Abs(actual - expected) < double.Epsilon);
        }
    }
}
