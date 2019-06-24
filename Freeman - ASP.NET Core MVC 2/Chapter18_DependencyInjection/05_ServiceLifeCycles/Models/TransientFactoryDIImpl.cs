using System;

namespace ServiceLifeCycles.Models
{
    public class TransientFactoryDIImpl : ITransientFactoryDI
    {
        public TransientFactoryDIImpl()
        {
            var random = new Random();
            var randomNumber = random.Next(0, 99);

            Guid = "Gotcha! Random number value is: " + randomNumber;
        }

        public string Guid { get; }
    }
}
