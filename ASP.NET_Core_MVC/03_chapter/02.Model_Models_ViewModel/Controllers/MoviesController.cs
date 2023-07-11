using Microsoft.AspNetCore.Mvc;
using Model_Models_ViewModel.Models;
using Model_Models_ViewModel.ViewModels;

namespace Model_Models_ViewModel.Controllers;

[Route("movies")]
public class MoviesController : Controller
{
    [HttpGet("movie")]
    public IActionResult Movie()
    {
        var movie = new Movie
        {
            Title = "Теория большого взрыва",
            Genre = "Ситком",
            Duration = 20
        };

        return View(movie);
    }

    [HttpGet("movies")]
    public IActionResult Movies()
    {
        var movies = new[]
        {
            new Movie
            {
                Title = "Теория большого взрыва",
                Genre = "Ситком",
                Duration = 20,
            },
            new Movie
            {
                Title = "Доктор Кто",
                Genre = "Фантастика",
                Duration = 25,
            },
            new Movie
            {
                Title = "Игра престолов",
                Genre = "Фэнтези",
                Duration = 63,
            },
        };

        return View(movies);
    }

    [HttpGet("group")]
    public IActionResult Group()
    {
        var comedy = new[]
        {
            new Comedy
            {
                Title = "Теория большого взрыва",
                VideoFormat = "1080i (HDTV)",
            },
            new Comedy
            {
                Title = "Один дома",
                VideoFormat = "1080i (HDTV)",
            }
        };

        var drama = new[]
        {
            new Drama
            {
                Title = "Зеленая миля",
                VideoFormat = "1080i (HDTV)",
                SoundFormat = "Dolby Digital",
            },
            new Drama
            {
                Title = "Джокер",
                VideoFormat = "1080i (HDTV)",
                SoundFormat = "Dolby Digital",
            }
        };

        var vm = new MoviesViewModel
        {
            ComedyList = comedy,
            DramaList = drama,
        };

        return View(vm);


    }
}
