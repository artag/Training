using System.Collections.Generic;

namespace DependencyChains.Models
{
    public class MemoryRepository : IRepository
    {
        private readonly IModelStorage _storage;

        public MemoryRepository(IModelStorage storage)
        {
            _storage = storage;

            new List<Product>
            {
                new Product { Name = "Kayak", Price = 275M },
                new Product { Name = "Lifejacket", Price = 48.95M },
                new Product { Name = "Soccer ball", Price = 19.50M }
            }.ForEach(product => AddProduct(product));
        }

        public IEnumerable<Product> Products => _storage.Items;

        public Product this[string name] => _storage[name];

        public void AddProduct(Product product) =>
            _storage[product.Name] = product;

        public void DeleteProduct(Product product) =>
            _storage.RemoveItem(product.Name);
    }
}
