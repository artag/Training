using System.Collections.Generic;

namespace WorkingWithVisualStudioTests.Models
{
    public interface IRepository
    {
        IEnumerable<Product> Products { get; }
        void AddProduct(Product product);
    }
}
