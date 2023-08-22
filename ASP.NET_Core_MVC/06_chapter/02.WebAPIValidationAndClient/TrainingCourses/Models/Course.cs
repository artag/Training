using System.ComponentModel.DataAnnotations;

namespace TrainingCourses.Models;

/// <summary>
/// Курс.
/// </summary>
public class Course
{
    /// <summary>
    /// Идентификатор курса.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название курса.
    /// </summary>
    [Required(ErrorMessage = "Укажите наименование курса")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Количество часов.
    /// </summary>
    [Required(ErrorMessage = "Укажите количество часов")]
    [Range(1, 300, ErrorMessage = "Количество часов должно быть в промежутке от 1 до 300")]
    public int Hours { get; set; }
}
