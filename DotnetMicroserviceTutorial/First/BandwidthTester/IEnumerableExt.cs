using System;
using System.Collections.Generic;

namespace BandwidthTester
{
    internal static class IEnumerableExt
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }

            return enumerable;
        }
    }
}
