using Microsoft.Extensions.Configuration;

namespace UpdateCurrency;

internal class Program
{
    static void Main(string[] args)
    {
        var connectionString = LoadConnectionString();
    }

    private static string LoadConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        return configuration.GetConnectionString("CommerceConnectionString");
    }

    private static CurrencyParser CreateCurrencyParser(string connectionString)
    {
        var provider = new SqlExchangeRateProvider(new CommerceContext(connectionString));
        return new CurrencyParser(provider);
    }
}
