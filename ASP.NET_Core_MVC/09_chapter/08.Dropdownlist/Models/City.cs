namespace Dropdownlist.Models;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public int PopulationSize { get; set; }
}
