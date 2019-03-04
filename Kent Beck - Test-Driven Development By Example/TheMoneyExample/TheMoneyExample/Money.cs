namespace TheMoneyExample
{
    public class Money
    {
        public override bool Equals(object obj)
        {
            var money = (Money)obj;
            return Amount == money.Amount &&
                   this.GetType().Equals(obj.GetType());
        }

        protected int Amount { get; set; }
    }
}
