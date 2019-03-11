using System.Collections.Generic;

namespace TheMoneyExample
{
    public class CurrencyExchangeRepository
    {
        private readonly Dictionary<CurrencyPair, double> _rates =
            new Dictionary<CurrencyPair, double>();

        public IReadOnlyDictionary<CurrencyPair, double> Rates => _rates;

        public void AddExchangeRate(Currency srcCurrency, Currency dstCurrency, double rate)
        {
            var pair = new CurrencyPair(srcCurrency, dstCurrency);
            _rates.Add(pair, rate);

            var reversePair = new CurrencyPair(dstCurrency, srcCurrency);
            _rates.Add(reversePair, 1.0/rate);
        }

        public double GetExchangeRate(Currency srcCurrency, Currency dstCurrency)
        {
            var pair = new CurrencyPair(srcCurrency, dstCurrency);
            return _rates[pair];
        }
    }
}
