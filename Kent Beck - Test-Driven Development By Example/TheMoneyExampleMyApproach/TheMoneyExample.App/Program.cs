using System;

namespace TheMoneyExample.App
{
    class Program
    {
        static void Main(string[] args)
        {
            // IBM
            var ibmTotal = GetTotalPrice(1000, 25, Currency.USD);

            // Novartis
            var novartisTotal = GetTotalPrice(400, 150, Currency.CHF);

            var calculation = GetCalculation();
            var total = calculation.Sum(ibmTotal, novartisTotal, Currency.USD);

            Console.WriteLine($"IBM Total:      {ibmTotal}");
            Console.WriteLine($"Novartis Total: {novartisTotal}");
            Console.WriteLine("---");
            Console.WriteLine($"Total amount:   {total}");
        }

        static Money GetTotalPrice(int numberOfShares, double priceOfShare, Currency currency)
        {
            var calculation = GetCalculation();
            return calculation.Product(new Money(priceOfShare, currency), numberOfShares, currency);
        }

        static Calculation GetCalculation()
        {
            var repository = new CurrencyExchangeRepository();
            repository.AddExchangeRate(Currency.USD, Currency.CHF, 1.5);
            var exchanger = new CurrencyExchanger(repository);

            return new Calculation(exchanger);
        }
    }
}
