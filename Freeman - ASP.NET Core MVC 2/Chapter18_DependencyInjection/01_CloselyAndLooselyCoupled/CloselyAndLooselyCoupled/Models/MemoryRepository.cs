using System.Collections.Generic;

namespace CloselyAndLooselyCoupled.Models
{
    public class MemoryRepository : IRepository
    {
        private Dictionary<string, Product> _products;

        public MemoryRepository()
        {
            _products = new Dictionary<string, Product>();

            new List<Product>
            {
                new Product { Name = "Kayak", Price = 275M },
                new Product { Name = "Lifejacket", Price = 48.95M },
                new Product { Name = "Soccer ball", Price = 19.50M }
            }.ForEach(product => AddProduct(product));
        }

        public IEnumerable<Product> Products => _products.Values;

        public Product this[string name] => _products[name];

        public void AddProduct(Product product) =>
            _products[product.Name] = product;

        public void DeleteProduct(Product product) =>
            _products.Remove(product.Name);
    }
}
