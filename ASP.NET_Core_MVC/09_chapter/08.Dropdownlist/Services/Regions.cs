using Dropdownlist.Models;

namespace Dropdownlist.Services;

public class Regions : IRegions
{
    public Regions()
    {
        Countries = new List<Country>
        {
            new Country() { Id = 1, Name = "Россия" },
            new Country() { Id = 2, Name = "Белоруссия" },
            new Country() { Id = 3, Name = "Франция" },
        };

        Cities = new List<City>
        {
            new City { Id = 1, Name = "Москва", CountryId = 1, PopulationSize = 11920000 },
            new City { Id = 2, Name = "Ростов-на-Дону", CountryId = 1, PopulationSize = 1100000 },
            new City { Id = 3, Name = "Ставрополь", CountryId = 1, PopulationSize = 408000 },
            new City { Id = 4, Name = "Краснодар", CountryId = 1, PopulationSize = 773000 },
            new City { Id = 5, Name = "Минск", CountryId = 2, PopulationSize = 2009786 },
            new City { Id = 6, Name = "Париж", CountryId = 3, PopulationSize = 2160000 },
            new City { Id = 7, Name = "Марсель", CountryId = 3, PopulationSize = 861635 },
        };
    }

    public List<Country> Countries { get; }
    public List<City> Cities { get; }
}
