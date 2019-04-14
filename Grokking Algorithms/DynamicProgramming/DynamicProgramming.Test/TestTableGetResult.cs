using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicProgramming.Test
{
    [TestClass]
    public class TestTableGetResult
    {
        [TestMethod]
        public void FillTable_GetLastCell()
        {
            // Arrange
            var player = new Item("Player", 3000, 4, "$", "pounds");
            var notebook = new Item("Notebook", 2000, 3, "$", "pounds");
            var guitar = new Item("Guitar", 1500, 1, "$", "pounds");

            var items = new List<Item>
            {
                player,
                notebook,
                guitar,
            };

            var maxWeight = 4;

            var tableController = new TableController(items, maxWeight);

            var expected = new List<Item> { guitar, notebook };

            // Act
            var result = tableController.GetResult().ToList();

            // Assert
            CollectionAssert.AreEquivalent(expected, result);
        }
    }
}
