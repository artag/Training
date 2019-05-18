using System.Collections.Generic;
using WorkingWithVisualStudioTests.Models;

namespace WorkingWithVisualStudioTests.Tests
{
    public class FakeRepository : IRepository
    {
        public FakeRepository(IEnumerable<Product> products)
        {
            Products = products;
        }

        public FakeRepository()
        {
            Products = new Product[]
            {
                new Product { Name = "P1", Price = 275M },
                new Product { Name = "P2", Price = 48.95M },
                new Product { Name = "P3", Price = 19.50M },
                new Product { Name = "P4", Price = 34.95M },
            };
        }

        public IEnumerable<Product> Products { get; }

        public void AddProduct(Product product)
        {
        }
    }
}
