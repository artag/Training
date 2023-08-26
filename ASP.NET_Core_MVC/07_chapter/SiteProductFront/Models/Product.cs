using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace SiteProduct.Models;

/// <summary>
/// Товар.
/// </summary>
public class Product
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Наименование.
    /// </summary>
    [Required(ErrorMessage = "Введите наименование")]
    [MinLength(3, ErrorMessage = "Наименование должно содержать не менее трех символов")]
    [MaxLength(50, ErrorMessage = "Наименование должно содержать не более трех символов")]
    [Display(Name = "Наименование")]
    //[DataType(DataType.Password)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Цена.
    /// </summary>
    [Required(ErrorMessage = "Введите цену товара")]
    [Range(0.01d, 10000.00d, ErrorMessage = "Цена товара должна быть от 1 копейки до 10 000 рублей")]
    [Display(Name = "Цена")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    /// <summary>
    /// Дата изготовления.
    /// </summary>
    [Required(ErrorMessage = "Введите дату изготовления товара")]
    [Display(Name = "Дата изготовления")]
    [DataType(DataType.Date)]
    public DateTime ProductionDate { get; set; }

    /// <summary>
    /// Идентификатор категории товара.
    /// </summary>
    [Required(ErrorMessage = "Введите категорию товара")]
    [Display(Name = "Категория")]
    [DataType(DataType.Text)]
    public int CategoryId { get; set; }
}
