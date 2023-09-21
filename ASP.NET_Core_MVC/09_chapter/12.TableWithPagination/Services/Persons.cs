using System.Net.Http.Headers;
using TablePage.Models;

namespace TablePage.Services;

public class Persons : IPersons
{
    private readonly Person[] _data;

    public Persons()
    {
        _data = new[]
        {
            new Person
            {
                Id = 1,
                FirstName = "Александр",
                LastName = "Кузнецов",
                BirthDate = new DateTime(1978, 02, 01),
                Country = "Россия"
            },
            new Person
            {
                Id = 2,
                FirstName = "Ирина",
                LastName = "Соколова",
                BirthDate = new DateTime(1987, 02, 02),
                Country = "Казахстан"
            },
            new Person
            {
                Id = 3,
                FirstName = "Иван",
                LastName = "Петров",
                BirthDate = new DateTime(1986, 03, 05),
                Country = "Франция"
            },
            new Person
            {
                Id = 4,
                FirstName = "Влад",
                LastName = "Семенов",
                BirthDate = new DateTime(1995, 02, 12),
                Country = "Россия"
            },
            new Person
            {
                Id = 5,
                FirstName = "Максим",
                LastName = "Андреев",
                BirthDate = new DateTime(2001, 01, 01),
                Country = "Германия"
            },
            new Person
            {
                Id = 6,
                FirstName = "Василий",
                LastName = "Иванов",
                BirthDate = new DateTime(1976, 12, 07),
                Country = "Россия"
            },
            new Person
            {
                Id = 7,
                FirstName = "Петр",
                LastName = "Сергеев",
                BirthDate = new DateTime(1980, 11, 15),
                Country = "Германия"
            },
            new Person
            {
                Id = 8,
                FirstName = "Борис",
                LastName = "Еремин",
                BirthDate = new DateTime(1956, 07, 06),
                Country = "Турция"
            },
            new Person
            {
                Id = 9,
                FirstName = "Елена",
                LastName = "Новикова",
                BirthDate = new DateTime(1967, 08, 12),
                Country = "Франция"
            },
            new Person
            {
                Id = 10,
                FirstName = "Татьяна",
                LastName = "Короткова",
                BirthDate = new DateTime(1986, 08, 03),
                Country = "Греция"
            },
            new Person
            {
                Id = 11,
                FirstName = "Владимир",
                LastName = "Комаров",
                BirthDate = new DateTime(1978, 09, 14),
                Country = "Германия"
            },
            new Person
            {
                Id = 12,
                FirstName = "Андрей",
                LastName = "Волков",
                BirthDate = new DateTime(1989, 05, 15),
                Country = "Болгария"
            },
        };
    }

    public Person[] GetAll()
    {
        return _data;
    }
}
