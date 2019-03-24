using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sort
{
    public class SelectionSort
    {
        private int[] _sorted;

        public IEnumerable<int> Do(IEnumerable<int> unsorted)
        {
            _sorted = unsorted.ToArray();
            DisplayItems();

            var length = _sorted.Length;
            for (var i = 0; i < length - 1; i++)
            {
                var minIndex = FindMinimalItemIndex(i, length);
                SwapElements(minIndex, i);
                DisplayItems();
            }

            return _sorted;
        }

        private int FindMinimalItemIndex(int i, int length)
        {
            var minIndex = i;

            for (var j = i + 1; j < length; j++)
                if (_sorted[minIndex] > _sorted[j])
                    minIndex = j;

            return minIndex;
        }

        private void SwapElements(int idx1, int idx2)
        {
            var tmp = _sorted[idx1];
            _sorted[idx1] = _sorted[idx2];
            _sorted[idx2] = tmp;
        }

        [Conditional("DEBUG")]
        private void DisplayItems()
        {
            foreach (var item in _sorted)
                Console.Write(item + " ");

            Console.WriteLine();
        }
    }
}
