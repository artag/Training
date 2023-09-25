using TableCrud.Models;

namespace TableCrud.Services;

public class Countries : ICountries
{
    private readonly List<Country> _data;

    public Countries()
    {
        _data = new List<Country>
        {
            new Country() { Id = 1, Name = "Россия"},
            new Country() { Id = 2, Name = "Франция"},
            new Country() { Id = 3, Name = "Норвегия"},
        };
    }

    public IReadOnlyCollection<Country> Get()
    {
        return _data;
    }

    public void Edit(Country country)
    {
        _data
            .Where(c => c.Id == country.Id)
            .ToList()
            .ForEach(c => c.Name = country.Name);
    }

    public void Delete(Country country)
    {
        _data.RemoveAll(c => c.Id == country.Id);
    }

    public int Add(Country country)
    {
        var exists = _data.Count(c => c.Name == country.Name) > 0;
        if (exists)
            throw new InvalidOperationException($"Страна {country.Name} уже существует в списке");

        country.Id = _data.Any()
            ? _data.Max(d => d.Id) + 1
            : 1;

        _data.Add(country);

        return country.Id;
    }
}
