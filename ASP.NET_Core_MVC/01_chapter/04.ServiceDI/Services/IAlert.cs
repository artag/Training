namespace AlertService.Services;

/// <summary>
/// Оповещение.
/// </summary>
public interface IAlert
{
    /// <summary>
    /// Возвращает строку оповещения.
    /// </summary>
    /// <returns>Строка оповещения.</returns>
    string GetMessage();
}
