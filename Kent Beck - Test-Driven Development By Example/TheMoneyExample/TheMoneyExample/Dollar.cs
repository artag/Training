namespace TheMoneyExample
{
    public class Dollar
    {
        private int _amount;

        public Dollar(int amount)
        {
            _amount = amount;
        }

        public Dollar Times(int multiplier)
        {
            return new Dollar(_amount * multiplier);
        }

        public override bool Equals(object obj)
        {
            var dollar = (Dollar)obj;
            return _amount == dollar._amount;
        }
    }
}
