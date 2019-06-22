using System.Collections.Generic;

namespace CloselyAndLooselyCoupled.Models
{
    public class AlternateRepository : IRepository
    {
        private Dictionary<string, Product> _products;

        public AlternateRepository()
        {
            _products = new Dictionary<string, Product>();

            new List<Product>
            {
                new Product { Name = "Corner Flags", Price = 34.95M },
                new Product { Name = "Stadium", Price = 79500M }
            }.ForEach(product => AddProduct(product));
        }

        public IEnumerable<Product> Products => _products.Values;

        public Product this[string name] => _products[name];

        public void AddProduct(Product product)
        {
            _products[product.Name] = product;
        }

        public void DeleteProduct(Product product)
        {
            _products.Remove(product.Name);
        }
    }
}
