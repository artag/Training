namespace TheMoneyExample
{
    public class Money
    {
        public override bool Equals(object obj)
        {
            var money = (Money)obj;
            return Amount == money.Amount;
        }

        protected int Amount { get; set; }
    }
}
