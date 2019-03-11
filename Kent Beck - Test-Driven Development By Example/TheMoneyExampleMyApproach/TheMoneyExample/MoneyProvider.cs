namespace TheMoneyExample
{
    public static class MoneyProvider
    {
        public static Money Dollar(int amount)
        {
            return new Money(amount, Currency.USD);
        }

        public static Money Franc(int amount)
        {
            return new Money(amount, Currency.CHF);
        }
    }
}
