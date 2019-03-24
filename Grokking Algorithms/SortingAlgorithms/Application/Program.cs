using System;
using System.Collections.Generic;
using Sort;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            var unsorted = new[]
                { 9, -5, 13, 0, 6, -4, -10, 0, 20, -10, 10, -7, 14, 15, 8, 7, -1, 3, 2, -2 };

            DoSelectionSort(unsorted);
            Console.WriteLine();
            DoQuickSort(unsorted);
        }

        static void DoSelectionSort(IEnumerable<int> unsorted)
        {
            Console.WriteLine("Selection sort:");
            var selectionSort = new SelectionSort();
            var sorted = selectionSort.Do(unsorted);
        }

        static void DoQuickSort(IEnumerable<int> unsorted)
        {
            Console.WriteLine("Quick sort:");
            var quickSort = new QuickSort();
            var sorted = quickSort.Do(unsorted);
        }

        static void Display(IEnumerable<int> sorted)
        {
            foreach (var item in sorted)
                Console.Write(item + " ");

            Console.WriteLine();
        }
    }
}
