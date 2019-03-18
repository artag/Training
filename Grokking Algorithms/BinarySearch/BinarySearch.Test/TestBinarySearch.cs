using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySearch.Test
{
    [TestClass]
    public class TestBinarySearch
    {
        private readonly BinarySearch _binarySearch;
        private readonly IEnumerable<int> _input;

        public TestBinarySearch()
        {
            _binarySearch = new BinarySearch();
            _input = new[] { 1, 3, 5, 7, 9, 11 };
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(2)]
        [DataRow(4)]
        [DataRow(6)]
        [DataRow(8)]
        [DataRow(10)]
        [DataRow(12)]
        public void Search_IndexOf_NonExistedItem_ReturnNull(int item)
        {
            Assert.AreEqual(null, _binarySearch.Run(_input, item));
        }

        [TestMethod]
        public void Search_IndexOf_11()
        {
            Assert.AreEqual(5, _binarySearch.Run(_input, 11));
        }

        [TestMethod]
        public void Search_IndexOf_9()
        {
            Assert.AreEqual(4, _binarySearch.Run(_input, 9));
        }

        [TestMethod]
        public void Search_IndexOf_7()
        {
            Assert.AreEqual(3, _binarySearch.Run(_input, 7));
        }

        [TestMethod]
        public void Search_IndexOf_5()
        {
            Assert.AreEqual(2, _binarySearch.Run(_input, 5));
        }

        [TestMethod]
        public void Search_IndexOf_3()
        {
            Assert.AreEqual(1, _binarySearch.Run(_input, 3));
        }

        [TestMethod]
        public void Search_IndexOf_1()
        {
            Assert.AreEqual(0, _binarySearch.Run(_input, 1));
        }
    }
}
