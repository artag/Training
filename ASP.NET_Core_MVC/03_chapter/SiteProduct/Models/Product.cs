namespace SiteProduct.Models;

/// <summary>
/// Товар.
/// </summary>
public class Product
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Наименование.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Цена.
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    /// Дата изготовления.
    /// </summary>
    public DateTime ProductionDate { get; init; }

    /// <summary>
    /// Идентификатор категории товара.
    /// </summary>
    public int CategoryId { get; init; }
}
