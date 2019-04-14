using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicProgramming.Test
{
    [TestClass]
    public class TestGetRemainingWeightIndex
    {
        private readonly WeightsService _weightsService;

        public TestGetRemainingWeightIndex()
        {
            var guitar = new Item("guitar", 1500, 1, "$", "pounds");
            var player = new Item("player", 3000, 4, "$", "pounds");
            var notebook = new Item("notebook", 2000, 3, "$", "pounds");
            var items = new [] { guitar.Weight, player.Weight, notebook.Weight };
            var maxWeight = 4.0;

            _weightsService = new WeightsService(items, maxWeight);
        }

        [TestMethod]
        [DataRow(0, -1)]
        [DataRow(1, 0)]
        [DataRow(2, 1)]
        [DataRow(3, 2)]
        public void RemainingWeightIndex_After_Guitar(int currentIndex, double expected)
        {
            // Arrange
            var guitar = new Item("guitar", 1500, 1, "$", "pounds");

            // Act
            var actual = _weightsService.GetRemainingWeightIndex(currentIndex, guitar);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(0, -1)]
        [DataRow(1, -1)]
        [DataRow(2, -1)]
        [DataRow(3, -1)]
        public void RemainingWeightIndex_After_Player(int currentIndex, double expected)
        {
            // Arrange
            var player = new Item("player", 3000, 4, "$", "pounds");

            // Act
            var actual = _weightsService.GetRemainingWeightIndex(currentIndex, player);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(0, -1)]
        [DataRow(1, -1)]
        [DataRow(2, -1)]
        [DataRow(3, 0)]
        public void RemainingWeightIndex_After_Notebook(int currentIndex, double expected)
        {
            // Arrange
            var notebook = new Item("notebook", 2000, 3, "$", "pounds");

            // Act
            var actual = _weightsService.GetRemainingWeightIndex(currentIndex, notebook);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(0, -1)]
        [DataRow(1, -1)]
        [DataRow(2, 0)]
        [DataRow(3, 1)]
        public void RemainingWeightIndex_After_GuitarAndPhone(int currentIndex, double expected)
        {
            // Arrange
            var guitar = new Item("guitar", 1500, 1, "$", "pounds");
            var phone = new Item("phone", 2000, 1, "$", "pounds");
            var items = new List<Item> { guitar, phone };

            // Act
            var actual = _weightsService.GetRemainingWeightIndex(currentIndex, items);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(0, -1)]
        [DataRow(1, -1)]
        [DataRow(2, -1)]
        [DataRow(3, 0)]
        public void RemainingWeightIndex_After_PhoneAndBook(int currentIndex, double expected)
        {
            // Arrange
            var guitar = new Item("guitar", 1500, 1, "$", "pounds");
            var book = new Item("book", 500, 2, "$", "pounds");
            var items = new List<Item> { guitar, book };

            // Act
            var actual = _weightsService.GetRemainingWeightIndex(currentIndex, items);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
