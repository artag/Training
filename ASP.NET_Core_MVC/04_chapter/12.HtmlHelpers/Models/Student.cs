using Microsoft.AspNetCore.Mvc.Rendering;

namespace HtmlHelpers.Models;

public class Student
{
    public int StudentId { get; init; }

    public string? Name { get; init; }

    public int Age { get; init; }

    public double Weight { get; init; }

    public string Description { get; init; } = string.Empty;

    public bool IsActive { get; init; }

    public string Gender { get; init; } = string.Empty;

    public Color FavouriteColor { get; init; }

    public Drink[] FavouriteDrinks { get; init; } = Array.Empty<Drink>();
    public IEnumerable<SelectListItem> AllDrinks { get; init; } = Array.Empty<SelectListItem>();

    public string Password { get; init; } = string.Empty;
}