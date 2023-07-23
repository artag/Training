using SiteProduct.ViewModels;

namespace SiteProduct.Models;

public static class ProductExtensions
{
    public static Product WithId(this Product product, int id) =>
        new Product
        {
            Id = id,
            Name = product.Name,
            Price = product.Price,
            ProductionDate = product.ProductionDate,
            CategoryId = product.CategoryId,
        };

    public static Product WithProduct(this Product product, Product changedProduct) =>
        new Product
        {
            Id = product.Id,
            Name = changedProduct.Name,
            Price = changedProduct.Price,
            ProductionDate = changedProduct.ProductionDate,
            CategoryId = changedProduct.CategoryId,
        };

    public static ProductViewModel MapToViewModel(this Product product, IReadOnlyCollection<ProductType> allCategories) =>
        new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            ProductionDate = product.ProductionDate,
            Category = GetCategory(product, allCategories)
        };

    private static string GetCategory(Product product, IReadOnlyCollection<ProductType> allCategories) =>
        allCategories
            .FirstOrDefault(category => category.Id == product.CategoryId)
            ?.TypeName ?? string.Empty;
}