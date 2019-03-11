namespace TheMoneyExample
{
    public class CurrencyExchanger
    {
        private readonly CurrencyExchangeRepository _repository;

        public CurrencyExchanger(CurrencyExchangeRepository repository)
        {
            _repository = repository;
        }

        public Money Exchange(Money srcMoney, Currency dstCurrency)
        {
            if (srcMoney.Currency == dstCurrency)
                return srcMoney;

            var rate = _repository.GetExchangeRate(srcMoney.Currency, dstCurrency);
            return new Money(srcMoney.Amount * rate, dstCurrency);
        }
    }
}
