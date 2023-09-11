namespace Authorization.Models;

/// <summary>
/// Пользователь.
/// </summary>
public class UserPortal
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Пароль.
    /// </summary>
    public string Password { get; set; }
}
