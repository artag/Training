using Authorization.Models;

namespace Authorization.Services;

/// <summary>
/// Репозиторий. Зарегистрированные пользователи системы.
/// </summary>
public interface IUsersPortalRepository
{
    /// <summary>
    /// Пользователи.
    /// </summary>
    UserPortal[] UsersPortal { get; }
}
