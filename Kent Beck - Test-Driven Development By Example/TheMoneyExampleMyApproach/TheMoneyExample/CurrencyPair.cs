namespace TheMoneyExample
{
    public struct CurrencyPair
    {
        public CurrencyPair(Currency sourceCurrency, Currency targetCurrency)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }
    }
}
