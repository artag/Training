using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sort.Test
{
    [TestClass]
    public class TestSelectionSort
    {
        private readonly SelectionSort _selectionSort;
        private readonly int[] _expected;

        public TestSelectionSort()
        {
            _selectionSort = new SelectionSort();
            _expected = new[] { -7, -5, -2, -1, 0, 2, 3, 6, 8, 10 };
        }

        [TestMethod]
        public void NoSort()
        {
            // Arrange
            var sorted = new[] { -7, -5, -2, -1, 0, 2, 3, 6, 8, 10 };

            // Act
            var actual = _selectionSort.Do(sorted);

            // Assert
            CollectionAssert.AreEqual(_expected, actual.ToArray());
        }

        [TestMethod]
        public void SwapFirstItems()
        {
            // Arrange
            var unsorted = new[] { -5, -7, -2, -1, 0, 2, 3, 6, 8, 10 };

            // Act
            var actual = _selectionSort.Do(unsorted);

            // Assert
            CollectionAssert.AreEqual(_expected, actual.ToArray());
        }

        [TestMethod]
        public void SwapLastItems()
        {
            // Arrange
            var unsorted = new[] { -7, -5, -2, -1, 0, 2, 3, 6, 10, 8 };

            // Act
            var actual = _selectionSort.Do(unsorted);

            // Assert
            CollectionAssert.AreEqual(_expected, actual.ToArray());
        }

        [TestMethod]
        public void Sort()
        {
            // Arrange
            var unsorted = new[] { -5, 6, 0, 10, -7, 8, -1, 3, 2, -2 };

            // Act
            var actual = _selectionSort.Do(unsorted);

            // Assert
            CollectionAssert.AreEqual(_expected, actual.ToArray());
        }
    }
}
