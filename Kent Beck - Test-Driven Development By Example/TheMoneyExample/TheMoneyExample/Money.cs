namespace TheMoneyExample
{
    public abstract class Money
    {
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
            return new Dollar(amount);
        }

        public static Franc Franc(int amount)
        {
            return new Franc(amount);
        }
    }
}