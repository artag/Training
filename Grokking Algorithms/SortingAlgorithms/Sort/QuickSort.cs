using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sort
{
    public class QuickSort
    {
        public IEnumerable<int> Do(IEnumerable<int> unsorted)
        {
            if (unsorted.Count() < 2)
                return unsorted;

            var pivot = unsorted.First();

            var less = unsorted.Skip(1).Where(item => item <= pivot);
            var greater = unsorted.Skip(1).Where(item => item > pivot);

            var union = Do(less).Append(pivot).Concat(Do(greater));
            DisplayItems(union);
            return union;
        }

        [Conditional("DEBUG")]
        private void DisplayItems(IEnumerable<int> arr)
        {
            foreach (var item in arr)
                Console.Write(item + " ");

            Console.WriteLine();
        }
    }
}
