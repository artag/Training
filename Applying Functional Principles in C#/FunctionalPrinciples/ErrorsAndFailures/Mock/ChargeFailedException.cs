using System;

namespace ErrorsAndFailures
{
    public class ChargeFailedException : Exception
    {
        public ChargeFailedException()
        {
        }

        public ChargeFailedException(string message)
            : base(message)
        {
        }

        public ChargeFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
