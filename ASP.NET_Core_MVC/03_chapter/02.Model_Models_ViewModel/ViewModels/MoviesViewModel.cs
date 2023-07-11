using Model_Models_ViewModel.Models;

namespace Model_Models_ViewModel.ViewModels;

/// <summary>
/// Модель представления фильма.
/// </summary>
public class MoviesViewModel
{
    /// <summary>
    /// Комедийные фильмы.
    /// </summary>
    public IEnumerable<Comedy> ComedyList { get; init; } = Enumerable.Empty<Comedy>();

    /// <summary>
    /// Драматические фильмы.
    /// </summary>
    public IEnumerable<Drama> DramaList { get; init; } = Enumerable.Empty<Drama>();
}
