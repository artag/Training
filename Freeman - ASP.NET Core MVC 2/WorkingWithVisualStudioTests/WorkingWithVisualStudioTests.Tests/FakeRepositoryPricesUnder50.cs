using System.Collections.Generic;
using WorkingWithVisualStudioTests.Models;

namespace WorkingWithVisualStudioTests.Tests
{
    public class FakeRepositoryPricesUnder50 : IRepository
    {
        public IEnumerable<Product> Products { get; } = new Product[]
        {
            new Product { Name = "P1", Price = 5M },
            new Product { Name = "P2", Price = 48.95M },
            new Product { Name = "P3", Price = 19.50M },
            new Product { Name = "P4", Price = 34.95M },
        };

        public void AddProduct(Product product)
        {
        }
    }
}
