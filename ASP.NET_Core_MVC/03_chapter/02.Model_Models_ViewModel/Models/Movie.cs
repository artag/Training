namespace Model_Models_ViewModel.Models;

/// <summary>
/// Фильм.
/// </summary>
public class Movie
{
    /// <summary>
    /// Заголовок.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Жанр.
    /// </summary>
    public string Genre { get; init; } = string.Empty;

    /// <summary>
    /// Длительность.
    /// </summary>
    public int Duration { get; init; }
}
