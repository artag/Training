using SiteProduct.Models;

namespace SiteProduct.ViewModels;

/// <summary>
/// Модель представления товара.
/// </summary>
public class ProductViewModel
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
    /// Категория товара.
    /// </summary>
    public string Category { get; init; } = string.Empty;


}
