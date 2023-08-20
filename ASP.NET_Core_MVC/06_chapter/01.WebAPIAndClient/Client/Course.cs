namespace Client;

/// <summary>
/// Курс.
/// </summary>
public class Course
{
    /// <summary>
    /// Идентификатор курса.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название курса.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Количество часов.
    /// </summary>
    public int Hours { get; set; }
}
