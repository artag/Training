using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicProgramming.Test
{
    [TestClass]
    public class TestItemEquality
    {
        [TestMethod]
        public void Item1_IsNull_Item2_IsNull_Equals()
        {
            // Arrange
            Item item1 = null;
            Item item2 = null;

            // Assert
            Assert.IsTrue(item1 == item2);
            Assert.IsFalse(item1 != item2);
        }

        [TestMethod]
        public void Item1_IsNull_Item2_IsNotNull_NotEquals()
        {
            // Arrange
            Item item1 = null;
            Item item2 = new Item("Guitar", 1500, 1, "$", "pounds");

            // Assert
            Assert.IsTrue(item1 != item2);
            Assert.IsFalse(item1 == item2);
        }

        [TestMethod]
        public void Item1_IsNotNull_Item2_IsNull_Equals()
        {
            // Arrange
            Item item1 = new Item("Guitar", 1500, 1, "$", "pounds");
            Item item2 = null;

            // Assert
            Assert.IsTrue(item1 != item2);
            Assert.IsFalse(item1 == item2);
        }

        [TestMethod]
        public void Item1_And_Item2_Equals()
        {
            // Arrange
            var player1 = new Item("Player", 3000, 4, "$", "pounds");
            var player2 = new Item("Player", 3000, 4, "$", "pounds");

            // Act
            var equals1 = player1.Equals(player2);
            var equals2 = player1 == player2;

            // Assert
            Assert.IsTrue(equals1);
            Assert.IsTrue(equals2);
        }

        [TestMethod]
        public void Item1_And_Item2_Different_Names_NotEquals()
        {
            // Arrange
            var player1 = new Item("Player1", 3000, 4, "$", "pounds");
            var player2 = new Item("Player2", 3000, 4, "$", "pounds");

            // Act
            var equals1 = player1.Equals(player2);
            var equals2 = player1 != player2;

            // Assert
            Assert.IsFalse(equals1);
            Assert.IsTrue(equals2);
        }

        [TestMethod]
        public void Item1_And_Item2_Different_Cost_NotEquals()
        {
            // Arrange
            var player1 = new Item("Player", 3000, 4, "$", "pounds");
            var player2 = new Item("Player", 3001, 4, "$", "pounds");

            // Act
            var equals1 = player1.Equals(player2);
            var equals2 = player1 != player2;

            // Assert
            Assert.IsFalse(equals1);
            Assert.IsTrue(equals2);
        }

        [TestMethod]
        public void Item1_And_Item2_Different_Weight_NotEquals()
        {
            // Arrange
            var player1 = new Item("Player", 3000, 4, "$", "pounds");
            var player2 = new Item("Player", 3000, 4.1, "$", "pounds");

            // Act
            var equals1 = player1.Equals(player2);
            var equals2 = player1 != player2;

            // Assert
            Assert.IsFalse(equals1);
            Assert.IsTrue(equals2);
        }

        [TestMethod]
        public void Item1_And_Item2_Different_CostName_NotEquals()
        {
            // Arrange
            var player1 = new Item("Player", 3000, 4, "$", "pounds");
            var player2 = new Item("Player", 3000, 4, "Rub", "pounds");

            // Act
            var equals1 = player1.Equals(player2);
            var equals2 = player1 != player2;

            // Assert
            Assert.IsFalse(equals1);
            Assert.IsTrue(equals2);
        }

        [TestMethod]
        public void Item1_And_Item2_Different_WeightName_NotEquals()
        {
            // Arrange
            var player1 = new Item("Player", 3000, 4, "$", "pounds");
            var player2 = new Item("Player", 3000, 4, "Rub", "pounds");

            // Act
            var equals1 = player1.Equals(player2);
            var equals2 = player1 != player2;

            // Assert
            Assert.IsFalse(equals1);
            Assert.IsTrue(equals2);
        }
    }
}
