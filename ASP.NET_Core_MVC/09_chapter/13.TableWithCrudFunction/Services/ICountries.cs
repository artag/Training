using TableCrud.Models;

namespace TableCrud.Services;

public interface ICountries
{
    IReadOnlyCollection<Country> Get();
    void Edit(Country country);
    void Delete(Country country);
    int Add(Country country);
}