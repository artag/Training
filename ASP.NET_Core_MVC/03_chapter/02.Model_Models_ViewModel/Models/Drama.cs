namespace Model_Models_ViewModel.Models;

/// <summary>
/// Драматический жанр.
/// </summary>
public class Drama
{
    /// <summary>
    /// Заголовок.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Формат видео.
    /// </summary>
    public string VideoFormat { get; init; } = string.Empty;

    /// <summary>
    /// Формат звука.
    /// </summary>
    public string SoundFormat { get; init; } = string.Empty;
}
