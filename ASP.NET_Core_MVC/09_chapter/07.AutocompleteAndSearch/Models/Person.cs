namespace Autocomplete.Models;

public class Person
{
    public int Id { get; set; }
    public string Fio { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }
    public string Country { get; set; } = string.Empty;
}
