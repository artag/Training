namespace TheMoneyExample
{
    public class Money
    {
        public Money(double amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public double Amount { get; }

        public Currency Currency { get; }

        public override string ToString()
        {
            return Amount + " " + Currency;
        }

        public override bool Equals(object obj)
        {
            var money = (Money)obj;
            return Amount == money.Amount && Currency == money.Currency;
        }
    }
}
