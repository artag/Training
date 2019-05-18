using System.Collections.Generic;
using WorkingWithVisualStudioTests.Models;

namespace WorkingWithVisualStudioTests.Tests
{
    public class FakeRepositoryPropertyOnceCall : IRepository
    {
        public IEnumerable<Product> Products
        {
            get
            {
                PropertyCounter++;
                return new[] { new Product { Name = "P1", Price = 100 }};
            }
        }

        public int PropertyCounter { get; private set; } = 0;

        public void AddProduct(Product product)
        {
        }
    }
}
