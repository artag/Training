using System.ComponentModel.DataAnnotations;

namespace HtmlHelpers.Models;

public class Book
{
    public int Id { get; init; }

    [Display(Name = "Наименование")]
    public string Title { get; init; } = string.Empty;

    [Display(Name = "Количество копий")]
    public int Copies { get; init; }
}
