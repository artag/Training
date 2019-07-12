using System.Collections.Generic;

namespace CreatingViewComponent.Models
{
    public class MemoryProductRepository : IProductRepository
    {
        private List<Product> _products = new List<Product>
        {
            new Product { Name = "Kayak", Price = 275M },
            new Product { Name = "Lifejacket", Price = 48.95M },
            new Product { Name = "Soccer ball", Price = 19.50M },
        };

        public IEnumerable<Product> Products => _products;

        public void AddProduct(Product newProduct)
        {
            _products.Add(newProduct);
        }
    }
}
