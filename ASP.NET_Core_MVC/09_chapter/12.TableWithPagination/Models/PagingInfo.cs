namespace TablePage.Models;

/// <summary>
/// Пагинация.
/// </summary>
public class PagingInfo
{
    /// <summary>
    /// Общее количество элементов для отображения.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Количество элементов на одной странице.
    /// </summary>
    public int ItemsPerPage { get; set; }

    /// <summary>
    /// Номер текущей страницы.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Общее количество страниц для отображения.
    /// </summary>
    public int TotalPages =>
        (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
}
