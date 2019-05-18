using System;
using System.Collections.Generic;

namespace WorkingWithVisualStudioTests.Tests
{
    public class Comparer
    {
        public static Comparer<U> Get<U>(Func<U, U, bool> func) => new Comparer<U>(func);
    }

    public class Comparer<T> : Comparer, IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparisonFunction;

        public Comparer(Func<T, T, bool> func)
        {
            _comparisonFunction = func;
        }

        public bool Equals(T x, T y) => _comparisonFunction(x, y);

        public int GetHashCode(T obj) => obj.GetHashCode();
    }
}
