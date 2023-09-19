using TableGroup.Models;

namespace TableGroup.Services;

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
                Login = "bono",
                LoginTime = new DateTime(2022, 05, 01, 16, 30, 0),
                ExitTime = new DateTime(2022, 05, 01, 16, 40, 0),
                OfficeId = 1
            },
            new Person
            {
                Id = 2,
                Login = "sasha",
                LoginTime = new DateTime(2022, 07, 02, 15, 30, 0),
                ExitTime = new DateTime(2022, 07, 02, 15, 35, 0),
                OfficeId = 3
            },
            new Person
            {
                Id = 3,
                Login = "masha",
                LoginTime = new DateTime(2022, 03, 05, 06, 30, 0),
                ExitTime = new DateTime(2022, 03, 05, 07, 20, 0),
                OfficeId = 2
            },
            new Person
            {
                Id = 4,
                Login = "katerina",
                LoginTime = new DateTime(2022, 04, 12, 12, 35, 0),
                ExitTime = new DateTime(2022, 04, 12, 12, 55, 0),
                OfficeId = 1
            },
            new Person
            {
                Id = 5,
                Login = "sveta",
                LoginTime = new DateTime(2022, 01, 01, 20, 00, 0),
                ExitTime = new DateTime(2022, 01, 01, 20, 05, 0),
                OfficeId = 2
            },
        };
    }

    public Person[] GetAll()
    {
        return _data;
    }
}
