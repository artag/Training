using Domain;
using Microsoft.Extensions.Logging;

namespace Repository.Implementations;

public class DummyData : IDisposable
{
    private readonly string _uid = Guid.NewGuid().ToString("D").Substring(0, 8);
    private readonly ILogger<DummyData> _log;

    public DummyData(ILogger<DummyData> log)
    {
        _log = log ?? throw new NullReferenceException(nameof(log));
        _log.LogInformation("Ctor '{0}', uid = '{1}'", nameof(DummyData), _uid);
    }

    public ICollection<Product> Products { get; } = new List<Product>
    {
        new Product(1, "T-shirt", "Color pink", 19M),
        new Product(2, "Shirt", "Color green", 18M),
        new Product(3, "Shoes", "Blue sweet shoes", 25M),
        new Product(4, "Coat", "White coat", 29M),
        new Product(5, "Dress", "Beautiful dress", 21M),
        new Product(6, "Hat", "Red hat", 11M),
        new Product(7, "Jeans", "Blue Jeans", 14M),
        new Product(8, "Trousers", "Nice trousers", 8M),
    };

    public void Dispose()
    {
        _log.LogInformation("Dispose '{0}', uid = '{1}'", nameof(DummyData), _uid);
    }
}