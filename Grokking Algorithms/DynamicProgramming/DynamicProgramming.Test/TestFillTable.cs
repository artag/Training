using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicProgramming.Test
{
    [TestClass]
    public class TestFillTable
    {
        private readonly TableController _tableController;
        private readonly Item _player;
        private readonly Item _notebook;
        private readonly Item _guitar;

        public TestFillTable()
        {
            _player = new Item("Player", 3000, 4, "$", "pounds");
            _notebook = new Item("Notebook", 2000, 3, "$", "pounds");
            _guitar = new Item("Guitar", 1500, 1, "$", "pounds");

            _tableController = new TableController();
        }

        [TestMethod]
        public void TestFillFirstRow_1()
        {
            // Arrange
            var items = new List<Item>
            {
                _player,
                _notebook,
                _guitar,
            };

            var maxWeight = 4;

            // Act
            _tableController.SetItemsAndInitTable(items, maxWeight);
            var table = _tableController.Table;

            // Assert
            CollectionAssert.AreEquivalent(table[0,0], new List<Item>());
            CollectionAssert.AreEquivalent(table[0,1], new List<Item>());
            CollectionAssert.AreEquivalent(table[0,2], new List<Item>());
            CollectionAssert.AreEquivalent(table[0,3], new List<Item> { _player });

            CollectionAssert.AreEquivalent(table[1,0], new List<Item>());
            CollectionAssert.AreEquivalent(table[1,1], new List<Item>());
            CollectionAssert.AreEquivalent(table[1,2], new List<Item> { _notebook });
            CollectionAssert.AreEquivalent(table[1,3], new List<Item> { _player });

            CollectionAssert.AreEquivalent(table[2,0], new List<Item> { _guitar });
            CollectionAssert.AreEquivalent(table[2,1], new List<Item> { _guitar });
            CollectionAssert.AreEquivalent(table[2,2], new List<Item> { _notebook });
            CollectionAssert.AreEquivalent(table[2,3], new List<Item> { _notebook, _guitar });
        }

        [TestMethod]
        public void TestFillFirstRow_2()
        {
            // Arrange
            var items = new List<Item>
            {
                _notebook,
                _guitar,
                _player,
            };

            var maxWeight = 4;

            // Act
            _tableController.SetItemsAndInitTable(items, maxWeight);
            var table = _tableController.Table;

            // Assert
            CollectionAssert.AreEquivalent(table[0,0], new List<Item>());
            CollectionAssert.AreEquivalent(table[0,1], new List<Item>());
            CollectionAssert.AreEquivalent(table[0,2], new List<Item> { _notebook });
            CollectionAssert.AreEquivalent(table[0,3], new List<Item> { _notebook });

            CollectionAssert.AreEquivalent(table[1,0], new List<Item> { _guitar });
            CollectionAssert.AreEquivalent(table[1,1], new List<Item> { _guitar });
            CollectionAssert.AreEquivalent(table[1,2], new List<Item> { _notebook });
            CollectionAssert.AreEquivalent(table[1,3], new List<Item> { _notebook, _guitar });

            CollectionAssert.AreEquivalent(table[2,0], new List<Item> { _guitar });
            CollectionAssert.AreEquivalent(table[2,1], new List<Item> { _guitar });
            CollectionAssert.AreEquivalent(table[2,2], new List<Item> { _notebook });
            CollectionAssert.AreEquivalent(table[2,3], new List<Item> { _notebook, _guitar });
        }

        [TestMethod]
        public void TestFillFirstRow_3()
        {
            // Arrange
            var items = new List<Item>
            {
                _guitar,
                _notebook,
                _player,
            };

            var maxWeight = 4;

            // Act
            _tableController.SetItemsAndInitTable(items, maxWeight);
            var table = _tableController.Table;

            // Assert
            CollectionAssert.AreEquivalent(table[0,0], new List<Item> { _guitar });
            CollectionAssert.AreEquivalent(table[0,1], new List<Item> { _guitar });
            CollectionAssert.AreEquivalent(table[0,2], new List<Item> { _guitar });
            CollectionAssert.AreEquivalent(table[0,3], new List<Item> { _guitar });

            CollectionAssert.AreEquivalent(table[1,0], new List<Item> { _guitar });
            CollectionAssert.AreEquivalent(table[1,1], new List<Item> { _guitar });
            CollectionAssert.AreEquivalent(table[1,2], new List<Item> { _notebook });
            CollectionAssert.AreEquivalent(table[1,3], new List<Item> { _notebook, _guitar });

            CollectionAssert.AreEquivalent(table[2,0], new List<Item> { _guitar });
            CollectionAssert.AreEquivalent(table[2,1], new List<Item> { _guitar });
            CollectionAssert.AreEquivalent(table[2,2], new List<Item> { _notebook });
            CollectionAssert.AreEquivalent(table[2,3], new List<Item> { _notebook, _guitar });
        }
    }
}
