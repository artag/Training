namespace TheMoneyExample
{
    public abstract class Money
    {
        protected string _currency;

        public Money(int amount, string currency)
        {
            _currency = currency;
            Amount = amount;
        }

        public override bool Equals(object obj)
        {
            var money = (Money)obj;
            return Amount == money.Amount &&
                   this.GetType().Equals(obj.GetType());
        }

        protected int Amount { get; set; }

        public abstract Money Times(int multiplier);

        public static Dollar Dollar(int amount)
        {
            return new Dollar(amount, "USD");
        }

        public static Franc Franc(int amount)
        {
            return new Franc(amount, "CHF");
        }
    }
}
