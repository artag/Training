namespace UpdateCurrency;

public class CurrencyParser
{
    private readonly IExchangeRateProvider _provider;

    public CurrencyParser(IExchangeRateProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public void Parse(string[] args)
    {
        decimal rate;
    }
}
