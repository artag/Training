namespace SiteProduct.Models;

/// <summary>
/// Категория товара.
/// </summary>
public class ProductType
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Категория товара.
    /// </summary>
    public string TypeName { get; init; } = string.Empty;
}
