using System.ComponentModel.DataAnnotations;

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
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    [Display(Name = "Наименование")]
    //[DataType(DataType.Password)]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Цена.
    /// </summary>
    [Required]
    [Range(0.01d, 10000.0d)]
    [Display(Name = "Цена")]
    [DataType(DataType.Currency)]
    public decimal Price { get; init; }

    /// <summary>
    /// Дата изготовления.
    /// </summary>
    [Required]
    [Display(Name = "Дата изготовления")]
    [DataType(DataType.Date)]
    public DateTime ProductionDate { get; init; }

    /// <summary>
    /// Идентификатор категории товара.
    /// </summary>
    [Required]
    [Display(Name = "Категория")]
    [DataType(DataType.Text)]
    public int CategoryId { get; init; }
}
