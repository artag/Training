using System.Collections.Generic;

namespace CloselyAndLooselyCoupled.Models
{
    public interface IRepository
    {
        IEnumerable<Product> Products { get; }

        Product this[string name] { get; }

        void AddProduct(Product product);
        void DeleteProduct(Product product);
    }
}
