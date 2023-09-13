using Dropdownlist.Models;

namespace Dropdownlist.Services;

public interface IRegions
{
    List<Country> Countries { get; }
    List<City> Cities { get; }
}