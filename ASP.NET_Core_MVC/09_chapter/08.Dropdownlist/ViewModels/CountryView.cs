using Dropdownlist.Models;

namespace Dropdownlist.ViewModels;

public class CountryView
{
    public int CountryId { get; set; }
    public List<City> Cities { get; set; } = new List<City>();
}
