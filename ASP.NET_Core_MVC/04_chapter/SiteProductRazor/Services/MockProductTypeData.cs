using SiteProduct.Models;

namespace SiteProduct.Services;

public class MockProductTypeData : IProductTypeData
{
    private readonly IReadOnlyCollection<ProductType> _productTypes;

    public MockProductTypeData()
    {
        _productTypes = new[]
        {
            new ProductType
            {
                Id = 1,
                TypeName = "Прочие",
            },
            new ProductType
            {
                Id = 2,
                TypeName = "Книги",
            },
            new ProductType
            {
                Id = 3,
                TypeName = "Комплектующие для компьютеров",
            },
            new ProductType
            {
                Id = 4,
                TypeName = "Смартфоны",
            },
        };
    }

    public IEnumerable<ProductType> GetAll()
    {
        return _productTypes;
    }
}
