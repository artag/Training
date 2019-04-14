using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicProgramming.Test
{
    [TestClass]
    public class TestWeightsInitialize
    {
        [TestMethod]
        [DataRow(1, 2, 3, 4)]
        [DataRow(4, 3, 1, 4)]
        [DataRow(2, 4, 1, 4)]
        [DataRow(3, 1, 4, 4)]
        public void Weights_From_1p0_To_4p0_Max_4p0(double a, double b, double c, double maxWeight)
        {
            // Arrange
            var weights = new[] { a, b, c };
            var expected = new double[] { 1, 2, 3, 4 };

            // Act
            var service = new WeightsService(weights, maxWeight);

            // Assert
            CollectionAssert.AreEqual(expected, service.Weights);
        }

        [TestMethod]
        [DataRow(0.5, 2, 1, 4)]
        [DataRow(4, 0.5, 1.5, 4)]
        [DataRow(1, 4, 0.5, 4)]
        [DataRow(0.5, 4, 5, 4)]
        public void Weights_From_0p5_To_4p0_Max_4p0(double a, double b, double c, double maxWeight)
        {
            // Arrange
            var weights = new[] { a, b, c };
            var expected = new double[] { 0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4 };

            // Act
            var service = new WeightsService(weights, maxWeight);

            // Assert
            CollectionAssert.AreEqual(expected, service.Weights);
        }

        [TestMethod]
        [DataRow(0.6, 0.9, 2.1)]
        [DataRow(0.6, 2.1, 2.1)]
        [DataRow(0.9, 0.6, 2.1)]
        [DataRow(0.9, 2.1, 2.1)]
        [DataRow(2.1, 0.6, 2.1)]
        [DataRow(2.1, 0.9, 2.1)]
        public void Weights_0p6_0p9_2p1_Max_2p1(double a, double b, double specifiedWeight)
        {
            // Arrange
            var weights = new[] { a, b };
            var expected = new double[] { 0.3, 0.6, 0.9, 1.2, 1.5, 1.8, 2.1 };

            // Act
            var service = new WeightsService(weights, specifiedWeight);

            // Assert
            CollectionAssert.AreEqual(expected, service.Weights);
        }

        [TestMethod]
        public void Weight_1p0_4p0()
        {
            // Arrange
            var weights = new[] { 1.0 };
            var specifiedWeight = 4;
            var expected = new double[] { 1.0, 2.0, 3.0, 4.0 };

            // Act
            var service = new WeightsService(weights, specifiedWeight);

            // Assert
            CollectionAssert.AreEqual(expected, service.Weights);
        }

        [TestMethod]
        public void Weight_2p0_4p0()
        {
            // Arrange
            var weights = new[] { 2.0 };
            var specifiedWeight = 4;
            var expected = new double[] { 2.0, 4.0 };

            // Act
            var service = new WeightsService(weights, specifiedWeight);

            // Assert
            CollectionAssert.AreEqual(expected, service.Weights);
        }

        [TestMethod]
        public void Weight_4p0_4p0()
        {
            // Arrange
            var weights = new[] { 4.0 };
            var specifiedWeight = 4;
            var expected = new double[] { 4.0 };

            // Act
            var service = new WeightsService(weights, specifiedWeight);

            // Assert
            CollectionAssert.AreEqual(expected, service.Weights);
        }
    }
}
