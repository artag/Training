# Легковес. Код.

## Простой пример

*(Не очень хорош)*

```csharp
// Defines Flyweight object that repeats itself.
public class FlyWeight
{
    public string CompanyName { get; set; }
    public string CompanyLocation { get; set; }
    public string CompanyWebSite { get; set; }
    //Bulky Data
    public byte[] CompanyLogo { get; set; }
}

public static class FlyWeightPointer
{
    public static readonly FlyWeight Company = new FlyWeight
    {
        CompanyName = "Abc",
        CompanyLocation = "XYZ",
        CompanyWebSite = "www.abc.com"
        // Load CompanyLogo here
    };
}

public class MyObject
{
    public string Name { get; set; }
    public string Company
    {
        get
        {
            return FlyWeightPointer.Company.CompanyName;
        }
    }
}
```
