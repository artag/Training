using System;
using System.Collections.Generic;

namespace Exceptions
{
    public class MyContext : IDisposable
    {
        public List<Customer> Customers { get; set; }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
