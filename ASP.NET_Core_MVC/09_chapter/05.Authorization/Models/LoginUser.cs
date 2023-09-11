using System.ComponentModel.DataAnnotations;

namespace Authorization.Models;

/// <summary>
/// Модель представления. Пользователь, который входит в систему.
/// </summary>
public class LoginUser
{
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    [Display(Name = "Ваше имя")]
    [Required(ErrorMessage = "Введите Ваше имя!")]
    [StringLength(50, ErrorMessage = "Введите не менее 1 и не более 50 символов!", MinimumLength = 1)]
    public string User { get; set; } = string.Empty;

    /// <summary>
    /// Пароль.
    /// </summary>
    [Display(Name = "Ваш пароль")]
    [Required(ErrorMessage = "Введите свой пароль!")]
    [StringLength(50, ErrorMessage = "Введите не менее 3 и не более 50 символов!", MinimumLength = 3)]
    public string Password { get; set; } = string.Empty;
}
