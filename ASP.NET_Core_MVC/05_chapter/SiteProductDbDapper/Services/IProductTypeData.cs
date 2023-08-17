using SiteProduct.Models;

namespace SiteProduct.Services;

public interface IProductTypeData
{
    IEnumerable<ProductType> GetAll();
}
