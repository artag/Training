using TableSort.Models;

namespace TableSort.Services;

public interface IPersons
{
    public IEnumerable<Person> GetAll();
}
