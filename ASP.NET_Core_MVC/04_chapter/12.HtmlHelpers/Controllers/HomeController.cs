using HtmlHelpers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HtmlHelpers.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewBag.Title = "Примеры использования HtmlHelper";
        ViewBag.Message = $"Message from {nameof(HomeController)} controller";
        return View();
    }

    public IActionResult Book(string message, int? magicNumber)
    {
        ViewBag.Message = message;
        ViewBag.MagicNumber = magicNumber!;
        ViewBag.Title = "Книга";
        return View();
    }

    public string Add(Book book)
    {
        return $"Книга {book.Title} в количестве {book.Copies} экз. добавлена";
    }

    public IActionResult CreateStudent()
    {
        ViewBag.Title = "Создать новый Student";
        var student = new Student
        {
            StudentId = GenerateStudentId(),
            AllDrinks = GetAllDrinks()
        };
        return View(student);
    }

    public IActionResult ViewNewStudent(Student student)
    {
        ViewBag.Title = "Новый Student";
        return View(student);
    }

    public int GenerateStudentId() =>
        new Random().Next(int.MaxValue);

    public IEnumerable<SelectListItem> GetAllDrinks()
    {
        var drinks = Enum.GetValues<Drink>().ToArray();
        var items = drinks
            .Select(dr => new SelectListItem { Text = dr.ToString("G"), Value = dr.ToString("G") })
            .ToList();
        return items;
    }
}
