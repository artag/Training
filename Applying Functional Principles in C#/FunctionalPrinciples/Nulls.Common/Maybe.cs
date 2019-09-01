using System;

namespace Nulls.Common
{
    public struct Maybe<T> : IEquatable<Maybe<T>> where T : class
    {
        private readonly T _value;

        private Maybe(T value)
        {
            _value = value;
        }

        public T Value
        {
            get
            {
                if (HasNoValue)
                    throw new InvalidOperationException();

                return _value;
            }
        }

        public bool HasValue => _value != null;

        public bool HasNoValue => !HasValue;

        /// <summary>
        /// Безопасное неявное преобразование T value -> Maybe&lt;T&gt;.
        /// </summary>
        /// <param name="value">Значение типа T.</param>
        /// <code>
        /// <example>
        /// Maybe&lt;string&gt; nullableString = new Maybe&lt;string&gt;("abc");
        /// Maybe&lt;string&gt; nullableString = "abc";
        /// Maybe&lt;string&gt; nullableString2 = null;
        /// </example>
        /// </code>
        public static implicit operator Maybe<T>(T value)
        {
            return new Maybe<T>(value);
        }

        /// <summary>
        /// Comparing nullable и nonNullable version of object.
        /// </summary>
        /// <param name="maybe">nullable version of object.</param>
        /// <param name="value">nonNullable version of object.</param>
        /// <returns>True - if equals, otherwise - false.</returns>
        /// <code>
        /// <example>
        /// Maybe&lt;string&gt; nullable = "abc";
        /// string nonNullable = "abc";
        /// bool equal = nullable == nonNullable;
        /// </example>
        /// </code>
        public static bool operator ==(Maybe<T> maybe, T value)
        {
            if (maybe.HasNoValue)
                return false;

            return maybe.Value.Equals(value);
        }

        public static bool operator !=(Maybe<T> maybe, T value)
        {
            return !(maybe == value);
        }

        /// <summary>
        /// Comparing two nullable instances of Maybe type.
        /// </summary>
        /// <param name="first">First instance.</param>
        /// <param name="second">Second instance.</param>
        /// <returns>True - if equals, otherwise - false.</returns>
        public static bool operator ==(Maybe<T> first, Maybe<T> second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Maybe<T> first, Maybe<T> second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Maybe<T>))
                return false;

            var other = (Maybe<T>)obj;
            return Equals(other);
        }

        public bool Equals(Maybe<T> other)
        {
            if (HasNoValue && other.HasNoValue)
                return true;

            if (HasNoValue || other.HasNoValue)
                return false;

            return _value.Equals(other._value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            if (HasNoValue)
                return "No value";

            return Value.ToString();
        }

        // Этот метод использовать исключительно для совместимости со сторонним кодом,
        // использующими null.
        public T Unwrap(T defaultValue = default(T))
        {
            if (HasValue)
                return Value;

            return defaultValue;
        }
    }
}
