using System.Collections.Generic;

namespace WorkingWithVisualStudioTests.Models
{
    public class SimpleRepository : IRepository
    {
        private static SimpleRepository _sharedRepository = new SimpleRepository();
        private Dictionary<string, Product> _products = new Dictionary<string, Product>();

        public SimpleRepository()
        {
            var initialItems = new[]
            {
                new Product { Name = "Kayak", Price = 275M },
                new Product { Name = "Lifejacket", Price = 48.95M },
                new Product { Name = "Soccer ball", Price = 19.50M },
                new Product { Name = "Corner flag", Price = 34.95M },
            };

            foreach (var initialItem in initialItems)
            {
                AddProduct(initialItem);
            }
        }

        public static SimpleRepository SharedRepository => _sharedRepository;

        public IEnumerable<Product> Products => _products.Values;

        public void AddProduct(Product product) => _products.Add(product.Name, product);
    }
}
