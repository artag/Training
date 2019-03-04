namespace TheMoneyExample
{
    public class Dollar : Money
    {
        public Dollar(int amount, string currency) : base(amount, currency)
        {
        }

        public string Currency => _currency;

        public override Money Times(int multiplier)
        {
            return Dollar(Amount * multiplier);
        }
    }
}
