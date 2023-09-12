using Autocomplete.Models;

namespace Autocomplete.Services;

public class Persons : IPersons
{
    public Persons()
    {
        People = new[]
        {
            new Person
            {
                Id = 1,
                Fio = "Фриман С.А.",
                Birthdate = new DateTime(1994, 12, 01),
                Country = "Россия",
            },
            new Person
            {
                Id = 1,
                Fio = "Фриман С.А.",
                Birthdate = new DateTime(1994, 12, 01),
                Country = "Россия",
            },
            new Person
            {
                Id = 2,
                Fio = "Новикова А.Е.",
                Birthdate = new DateTime(1991, 02, 15),
                Country = "Белоруссия",
            },
            new Person
            {
                Id = 3,
                Fio = "Николаев И.А.",
                Birthdate = new DateTime(1993, 08, 23),
                Country = "Россия",
            },
            new Person
            {
                Id = 4,
                Fio = "Кузнецов В.М.",
                Birthdate = new DateTime(1998, 11, 10),
                Country = "Казахстан",
            },
            new Person
            {
                Id = 5,
                Fio = "Григорьева Д.С.",
                Birthdate = new DateTime(2001, 12, 04),
                Country = "Россия",
            },
            new Person
            {
                Id = 6,
                Fio = "Фридт А.С.",
                Birthdate = new DateTime(1987, 06, 05),
                Country = "Германия",
            },
        };
    }

    public Person[] People { get; }
}
