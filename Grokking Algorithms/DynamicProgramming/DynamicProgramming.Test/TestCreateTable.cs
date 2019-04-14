using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicProgramming.Test
{
    [TestClass]
    public class TestCreateTable
    {
        [TestMethod]
        public void CreateTable_3x4()
        {
            // Arrange
            var items = new List<Item>
            {
                new Item("Player", 3000, 4, "$", "pounds"),
                new Item("Notebook", 2000, 3, "$", "pounds"),
                new Item("Guitar", 1500, 1, "$", "pounds"),
            };

            var maxWeight = 4;

            var table = new TableController(items, maxWeight);
            var expectedRows = 3;
            var expectedCols = 4;

            // Act
            var actualRows = table.RowCount;
            var actualCols = table.ColumnCount;

            // Assert
            Assert.AreEqual(expectedRows, actualRows);
            Assert.AreEqual(expectedCols, actualCols);
        }

        [TestMethod]
        public void CreateTable_2x4()
        {
            // Arrange
            var items = new List<Item>
            {
                new Item("Notebook", 2000, 3, "$", "pounds"),
                new Item("Guitar", 1500, 1, "$", "pounds"),
            };

            var maxWeight = 4;

            var table = new TableController(items, maxWeight);
            var expectedRows = 2;
            var expectedCols = 4;

            // Act
            var actualRows = table.RowCount;
            var actualCols = table.ColumnCount;

            // Assert
            Assert.AreEqual(expectedRows, actualRows);
            Assert.AreEqual(expectedCols, actualCols);
        }
    }
}
