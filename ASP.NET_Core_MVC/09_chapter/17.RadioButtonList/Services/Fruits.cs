using RadioButtonList.Models;

namespace RadioButtonList.Services;

public class Fruits : IFruits
{
    private readonly Fruit[] _fruits = new[]
    {
        new Fruit { Id = 1, Name = "Яблоки" },
        new Fruit { Id = 2, Name = "Сливы" },
        new Fruit { Id = 3, Name = "Груши" },
        new Fruit { Id = 4, Name = "Абрикос" },
        new Fruit { Id = 5, Name = "Манго" },
        new Fruit { Id = 6, Name = "Апельсин" },
    };

    public Fruit[] Get()
    {
        return _fruits;
    }
}
