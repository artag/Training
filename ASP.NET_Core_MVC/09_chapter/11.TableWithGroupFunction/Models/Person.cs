namespace TableGroup.Models;

public class Person
{
    public int Id { get; set; }
    public string Login { get; set; }
    public DateTime LoginTime { get; set; }
    public DateTime ExitTime { get; set; }
    public int OfficeId { get; set; }
}
