namespace AnemicAndFullModels.Models;

/// <summary>
/// Анемичная (тонкая) модель.
/// </summary>
public class Auto
{
    /// <summary>
    /// Наименование автопроизводителя.
    /// </summary>
    public string Brand { get; init; }

    /// <summary>
    /// Название модели.
    /// </summary>
    public string ModelAuto { get; init; }

    /// <summary>
    /// Объем двигателя.
    /// </summary>
    public int EngineCapacity { get; init; }
}
