﻿namespace TheMoneyExample.Test
{
    public class Bank
    {
        public Money Reduce(IExpression source, string to)
        {
            return source.Reduce(to);
        }
    }
}
