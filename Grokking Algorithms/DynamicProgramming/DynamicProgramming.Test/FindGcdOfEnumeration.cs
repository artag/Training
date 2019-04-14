using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicProgramming.Test
{
    [TestClass]
    public class FindGcdOfEnumeration
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gcd_For_NullValue_ThrowArgumentNullException()
        {
            var actual = GreatestCommonDivisor.Find(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Gcd_For_ZeroItems_ThrowArgumentOutOfRangeException()
        {
            var actual = GreatestCommonDivisor.Find(new List<double>());
        }

        [TestMethod]
        [DataRow(32, 32)]
        [DataRow(0.3, 0.3)]
        public void Gcd_For_OneNumber_Is_Number(double number, double expected)
        {
            var numbers = new[] { number };

            var actual = GreatestCommonDivisor.Find(numbers);
            Assert.IsTrue(Math.Abs(actual - expected) < Double.Epsilon);
        }

        [TestMethod]
        [DataRow(0.5, 1.0, 0.5)]
        [DataRow(1.0, 0.5, 0.5)]
        public void Gcd_For_0p5_1p5_Is_0p5(double a, double b, double expected)
        {
            var numbers = new[] { a, b };

            var actual = GreatestCommonDivisor.Find(numbers);
            Assert.IsTrue(Math.Abs(actual - expected) < double.Epsilon);
        }

        [TestMethod]
        [DataRow(32, 48, 64)]
        [DataRow(32, 64, 48)]
        [DataRow(48, 32, 64)]
        [DataRow(48, 64, 32)]
        [DataRow(64, 32, 48)]
        [DataRow(64, 48, 32)]
        public void Gcd_For_32_48_64_Is_16(double a, double b, double c)
        {
            var numbers = new[] { a, b, c };
            var expected = 16;

            var actual = GreatestCommonDivisor.Find(numbers);
            Assert.IsTrue(Math.Abs(actual - expected) < double.Epsilon);
        }

        [TestMethod]
        [DataRow(1.8, 2.4, 3)]
        [DataRow(1.8, 3, 2.4)]
        [DataRow(2.4, 1.8, 3)]
        [DataRow(2.4, 3, 1.8)]
        [DataRow(3, 1.8, 2.4)]
        [DataRow(3, 2.4, 1.8)]
        public void Gcd_For_1p8_2p4_3p0_Is_0p6(double a, double b, double c)
        {
            var numbers = new[] { a, b, c };
            var expected = 0.6;

            var actual = GreatestCommonDivisor.Find(numbers);
            Assert.IsTrue(Math.Abs(actual - expected) < double.Epsilon);
        }

        [TestMethod]
        [DataRow(1.2, 2.4, 3, 6)]
        [DataRow(1.2, 2.4, 6, 3)]
        [DataRow(1.2, 3, 2.4, 6)]
        [DataRow(1.2, 3, 6, 2.4)]
        [DataRow(1.2, 6, 2.4, 3)]
        [DataRow(1.2, 6, 3, 2.4)]
        [DataRow(2.4, 1.2, 3, 6)]
        [DataRow(2.4, 1.2, 6, 3)]
        [DataRow(2.4, 3, 1.2, 6)]
        [DataRow(2.4, 3, 6, 1.2)]
        [DataRow(2.4, 6, 1.2, 3)]
        [DataRow(2.4, 6, 3, 1.2)]
        [DataRow(3, 1.2, 3, 2.4)]
        [DataRow(3, 1.2, 2.4, 3)]
        [DataRow(3, 2.4, 1.2, 6)]
        [DataRow(3, 2.4, 6, 1.2)]
        [DataRow(3, 6, 1.2, 2.4)]
        [DataRow(3, 6, 2.4, 1.2)]
        [DataRow(6, 1.2, 2.4, 3)]
        [DataRow(6, 1.2, 3, 2.4)]
        [DataRow(6, 2.4, 1.2, 3)]
        [DataRow(6, 2.4, 3, 1.2)]
        [DataRow(6, 3, 1.2, 2.4)]
        [DataRow(6, 3, 2.4, 1.2)]
        public void Gcd_For_1p2_2p4_3p0_6p0_Is_0p6(double a, double b, double c, double d)
        {
            var numbers = new[] { a, b, c, d };
            var expected = 0.6;

            var actual = GreatestCommonDivisor.Find(numbers);
            Assert.IsTrue(Math.Abs(actual - expected) < double.Epsilon);
        }

        [TestMethod]
        [DataRow(0.8, 2.4, 1.4, 4.0, 0.2)]
        [DataRow(2.6, 0.2, 1.8, 3.6, 4.8)]
        [DataRow(2.6, 2.6, 0.2, 1.8, 4.8)]
        [DataRow(3.2, 3.2, 3.2, 0.2, 3.2)]
        public void Gcd_For_FiveNumbers(double a, double b, double c, double d, double e)
        {
            var numbers = new[] { a, b, c, d, e };
            var expected = 0.2;

            var actual = GreatestCommonDivisor.Find(numbers);
            Assert.IsTrue(Math.Abs(actual - expected) < double.Epsilon);
        }
    }
}
