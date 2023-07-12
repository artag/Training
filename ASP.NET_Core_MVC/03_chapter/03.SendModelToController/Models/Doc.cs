namespace SendModelToController.Models;

/// <summary>
/// Документ.
/// </summary>
public class Doc
{
    /// <summary>
    /// Код документа.
    /// </summary>
    public int Code { get; init; }

    /// <summary>
    /// Заголовок.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Кем выдан.
    /// </summary>
    public string IssuedBy { get; init; } = string.Empty;
}
