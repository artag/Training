namespace TheMoneyExample
{
    public class Money : IExpression
    {
        private int _amount;

        protected Money(int amount, string currency)
        {
            Currency = currency;
            _amount = amount;
        }

        public string Currency { get; }

        public Money Times(int multiplier)
        {
            return new Money(_amount * multiplier, Currency);
        }

        public static Money Dollar(int amount)
        {
            return new Money(amount, "USD");
        }

        public static Money Franc(int amount)
        {
            return new Money(amount, "CHF");
        }

        public override bool Equals(object obj)
        {
            var money = (Money)obj;
            return _amount == money._amount &&
                   Currency == money.Currency;
        }

        public override string ToString()
        {
            return _amount + " " + Currency;
        }

        public Money Plus(Money addent)
        {
            return new Money(_amount + addent._amount, Currency);
        }
    }
}
