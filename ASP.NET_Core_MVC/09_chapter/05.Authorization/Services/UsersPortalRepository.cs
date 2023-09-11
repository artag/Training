using Authorization.Models;

namespace Authorization.Services;

/// <summary>
/// Репозиторий. Зарегистрированные пользователи системы.
/// </summary>
public class UsersPortalRepository : IUsersPortalRepository
{
    public UsersPortalRepository()
    {
        UsersPortal = new[]
        {
            new UserPortal { Id = 1, UserName = "Admin", Password = "123" },
            new UserPortal { Id = 2, UserName = "Guest", Password = "321" },
        };
    }

    /// <inheritdoc />
    public UserPortal[] UsersPortal { get; }
}
