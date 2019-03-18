using System.Collections.Generic;
using System.Linq;

namespace BinarySearch
{
    public class BinarySearch
    {
        public int? Run(IEnumerable<int> input, int item)
        {
            var array = input.ToArray();
            var low = 0;
            var high = array.Length - 1;

            while (low <= high)
            {
                var mid = (low + high) / 2;
                var currentItem = array[mid];

                if (currentItem == item)
                    return mid;

                if (currentItem < item)
                    low = mid + 1;

                if (item < currentItem)
                    high = mid - 1;
            }

            return null;
        }
    }
}
