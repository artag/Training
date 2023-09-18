using TableSort.Infrastructure;

namespace TableSort.Models;

public class PersonView
{
    public IEnumerable<Person> Persons { get; init; } = null!;
    public SortStatus SortStatus { get; init; }
}
