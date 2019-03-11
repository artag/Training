namespace TheMoneyExample
{
    public class Calculation
    {
        private readonly CurrencyExchanger _currencyExchanger;

        public Calculation(CurrencyExchanger currencyExchanger)
        {
            _currencyExchanger = currencyExchanger;
        }

        public Money Sum(Money augend, Money addend, Currency targetCurrency)
        {
            var x = _currencyExchanger.Exchange(augend, targetCurrency);
            var y = _currencyExchanger.Exchange(addend, targetCurrency);
            var sum = x.Amount + y.Amount;

            return new Money(sum, targetCurrency);
        }

        public Money Product(Money money, double multiplier, Currency targetCurrency)
        {
            var x = _currencyExchanger.Exchange(money, targetCurrency);
            var product = x.Amount * multiplier;

            return new Money(product, targetCurrency);
        }
    }
}
