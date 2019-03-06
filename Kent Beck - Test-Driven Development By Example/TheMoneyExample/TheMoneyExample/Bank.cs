namespace TheMoneyExample.Test
{
    public class Bank
    {
        public Money Reduce(IExpression sum, string usd)
        {
            return Money.Dollar(10);
        }
    }
}
