using TableSort.Models;

namespace TableSort.Services;

public class Persons : IPersons
{
    private readonly Person[] _persons;

    public Persons()
    {
        _persons = new[]
        {
            new Person
            {
                Id = 1,
                FirstName = "Алексей",
                LastName = "Никитин",
                BirthDate = new DateTime(1978, 01, 02),
            },
            new Person
            {
                Id = 2,
                FirstName = "Ирина",
                LastName = "Кузнецова",
                BirthDate = new DateTime(1987, 02, 02),
            },
            new Person
            {
                Id = 3,
                FirstName = "Иван",
                LastName = "Петров",
                BirthDate = new DateTime(1986, 03, 05),
            },
            new Person
            {
                Id = 4,
                FirstName = "Влад",
                LastName = "Иванов",
                BirthDate = new DateTime(1995, 02, 12),
            },
            new Person
            {
                Id = 5,
                FirstName = "Максим",
                LastName = "Тарасов",
                BirthDate = new DateTime(2001, 01, 05),
            },
        };
    }

    public IEnumerable<Person> GetAll()
    {
        return _persons;
    }
}
