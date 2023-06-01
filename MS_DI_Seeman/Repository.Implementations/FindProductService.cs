using App.Interfaces;
using Common;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;

namespace Repository.Implementations;

public class FindProductService : IService<FindProductRequest, FindProductResponse>, IDisposable
{
    private readonly string _uid = Guid.NewGuid().ToString("D").Substring(0, 8);

    private readonly IService<GetConfigurationRequest, GetConfigurationResponse> _getConfiguration;
    private readonly ILogger<FindProductService> _log;
    private readonly DummyData _data;

    public FindProductService(
        IService<GetConfigurationRequest, GetConfigurationResponse> getConfiguration,
        ILogger<FindProductService> log,
        DummyData data)
    {
        _getConfiguration = getConfiguration ?? throw new ArgumentNullException(nameof(getConfiguration));
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _data = data ?? throw new ArgumentNullException(nameof(data));

        _log.LogInformation("Ctor '{0}', uid = '{1}'", nameof(FindProductService), _uid);
    }

    public Task<Result<FindProductResponse>> Execute(FindProductRequest request)
    {
        var product = _data.Products.First(p => p.Name == request.Name);
        var response = new FindProductResponse(product);
        return Task.FromResult(Result.Success(response));
    }

    public void Dispose()
    {
        _log.LogInformation("Dispose '{0}', uid = '{1}'", nameof(FindProductService), _uid);
    }
}