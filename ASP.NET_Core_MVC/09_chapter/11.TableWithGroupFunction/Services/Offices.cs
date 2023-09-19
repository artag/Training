using TableGroup.Models;

namespace TableGroup.Services;

public class Offices : IOffices
{
    private readonly Office[] _data;

    public Offices()
    {
        _data = new[]
        {
            new Office { Id = 1, Country = "Болгария", Name = "Офис в Софии" },
            new Office { Id = 2, Country = "Россия", Name = "Офис в Краснодаре" },
            new Office { Id = 3, Country = "Канада", Name = "Офис в Торонто" },
        };
    }

    public Office[] GetAll()
    {
        return _data;
    }
}
