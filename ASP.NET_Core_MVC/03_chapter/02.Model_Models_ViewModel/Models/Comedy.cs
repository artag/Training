namespace Model_Models_ViewModel.Models;

/// <summary>
/// Комедийный жанр.
/// </summary>
public class Comedy
{
    /// <summary>
    /// Заголовок.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Формат видео.
    /// </summary>
    public string VideoFormat { get; init; } = string.Empty;
}
