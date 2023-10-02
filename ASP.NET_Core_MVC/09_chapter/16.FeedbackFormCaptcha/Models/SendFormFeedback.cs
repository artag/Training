using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Feedback.Models;

/// <summary>
/// Форма обратной связи.
/// </summary>
public class SendFormFeedback
{
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    [Display(Name = "Ваше имя")]
    [Required(ErrorMessage = "Сообщите Ваше имя.")]
    [StringLength(50, ErrorMessage = "Пожалуйста, введите не более 50 символов.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Эелектронная почта пользователя.
    /// </summary>
    [Display(Name = "Ваш email")]
    [Required(ErrorMessage = "Пожалуйста, введите действующий email.")]
    [EmailAddress(ErrorMessage = "Введите правильный email")]
    [StringLength(50, ErrorMessage = "Пожалуйста, введите не более 50 символов.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Тема сообщения.
    /// </summary>
    [Display(Name = "Тема")]
    [Required(ErrorMessage = "Введите тему сообщения.")]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "Пожалуйста, введите не менее 8 и не более 50 символов.")]
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Текст сообщения для отправки.
    /// </summary>
    [Display(Name = "Ваше сообщение")]
    [Required(ErrorMessage = "Введите текст сообщения.")]
    [StringLength(2500, ErrorMessage = "Длина сообщения не должна превышать 2500 символов.")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Код безопасности.
    /// </summary>
    [Required(ErrorMessage = "Укажите код безопасности.")]
    [StringLength(6)]
    public string CaptchaCode { get; set; } = string.Empty;

    /// <summary>
    /// Ошибки заполнения полей на форме.
    /// </summary>
    public string? ModelError { get; set; }
}
