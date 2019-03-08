namespace TheMoneyExample
{
    public interface IExpression
    {
        Money Reduce(Bank source, string to);
    }
}
