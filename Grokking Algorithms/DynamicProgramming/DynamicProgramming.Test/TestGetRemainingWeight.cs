using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicProgramming.Test
{
    [TestClass]
    public class TestGetRemainingWeight
    {
        private readonly WeightsService _weightsService;

        public TestGetRemainingWeight()
        {
            var guitar = new Item("guitar", 1500, 1, "$", "pounds");
            var player = new Item("player", 3000, 4, "$", "pounds");
            var notebook = new Item("notebook", 2000, 3, "$", "pounds");
            var items = new [] { guitar.Weight, player.Weight, notebook.Weight };
            var maxWeight = 4.0;

            _weightsService = new WeightsService(items, maxWeight);
        }

        [TestMethod]
        [DataRow(0, 0.0)]
        [DataRow(1, 1.0)]
        [DataRow(2, 2.0)]
        [DataRow(3, 3.0)]
        public void RemainingWeight_After_Guitar(int index, double expected)
        {
            // Arrange
            var guitar = new Item("guitar", 1500, 1, "$", "pounds");

            // Act
            var actual = _weightsService.GetRemainingWeight(index, guitar);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(0, 0.0)]
        [DataRow(1, 0.0)]
        [DataRow(2, 0.0)]
        [DataRow(3, 0.0)]
        public void RemainingWeight_After_Player(int index, double expected)
        {
            // Arrange
            var player = new Item("player", 3000, 4, "$", "pounds");

            // Act
            var actual = _weightsService.GetRemainingWeight(index, player);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(0, 0.0)]
        [DataRow(1, 0.0)]
        [DataRow(2, 0.0)]
        [DataRow(3, 1.0)]
        public void RemainingWeight_After_Notebook(int index, double expected)
        {
            // Arrange
            var notebook = new Item("notebook", 2000, 3, "$", "pounds");

            // Act
            var actual = _weightsService.GetRemainingWeight(index, notebook);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
