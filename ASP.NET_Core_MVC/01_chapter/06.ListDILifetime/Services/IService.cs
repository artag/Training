namespace DILifetime.Services;

public interface IService
{
    /// <summary>
    /// Дата и время создания экземпляра сервиса.
    /// </summary>
    DateTime CreatedDateTime { get; }

    /// <summary>
    /// Уникальный идентификатор экземпляра сервиса.
    /// </summary>
    Guid Uid { get; }
}
