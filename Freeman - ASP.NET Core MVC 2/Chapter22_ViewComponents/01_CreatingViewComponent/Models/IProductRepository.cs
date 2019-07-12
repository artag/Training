using System.Collections.Generic;

namespace CreatingViewComponent.Models
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }

        void AddProduct(Product newProduct);
    }
}
