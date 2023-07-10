namespace AnemicAndFullModels.Models;

/// <summary>
/// Полная (богатая) модель.
/// </summary>
public class ThickAuto
{
    public ThickAuto(string brand, string modelAuto, int engineCapacity)
    {
        Brand = brand;
        ModelAuto = modelAuto;
        EngineCapacity = engineCapacity;
    }

    /// <summary>
    /// Наименование автопроизводителя.
    /// </summary>
    public string Brand { get; }

    /// <summary>
    /// Название модели.
    /// </summary>
    public string ModelAuto { get; }

    /// <summary>
    /// Об ъем двигателя.
    /// </summary>
    public int EngineCapacity { get; }

    public string GetInfo() =>
        $"Полная (богатая) модель {nameof(ThickAuto)}:\n" +
        $"Наименование: {Brand}, " +
        $"Модель: {ModelAuto}, " +
        $"Объем двигателя, куб. см: {EngineCapacity}.";
}
