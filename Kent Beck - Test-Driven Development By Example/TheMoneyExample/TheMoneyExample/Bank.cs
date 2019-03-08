using System.Collections.Generic;

namespace TheMoneyExample
{
    public class Bank
    {
        private Dictionary<Pair, int> _rates = new Dictionary<Pair, int>();

        public Money Reduce(IExpression source, string to)
        {
            return source.Reduce(this, to);
        }

        public void AddRate(string from, string to, int rate)
        {
            _rates.Add(new Pair(from, to), rate);
        }

        public int Rate(string from, string to)
        {
            if (from.Equals(to))
                return 1;

            var pair = new Pair(from, to);
            return _rates[pair];
        }
    }
}
