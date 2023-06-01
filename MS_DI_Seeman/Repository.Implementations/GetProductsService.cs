using App.Interfaces;
using Common;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;

namespace Repository.Implementations;

public class GetProductsService : IService<GetProductsRequest, GetProductsResponse>, IDisposable
{
    private readonly string _uid = Guid.NewGuid().ToString("D").Substring(0, 8);

    private readonly IService<GetConfigurationRequest, GetConfigurationResponse> _getConfigurationService;
    private readonly ILogger<GetProductsService> _log;
    private readonly DummyData _data;

    public GetProductsService(
        IService<GetConfigurationRequest, GetConfigurationResponse> getConfigurationService,
        ILogger<GetProductsService> log,
        DummyData data)
    {
        _getConfigurationService = getConfigurationService ?? throw new ArgumentNullException(nameof(getConfigurationService));
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _data = data ?? throw new ArgumentNullException(nameof(data));

        _log.LogInformation("Ctor '{0}', uid = '{1}'", nameof(GetProductsService), _uid);
    }

    public Task<Result<GetProductsResponse>> Execute(GetProductsRequest request)
    {
        var response = new GetProductsResponse(_data.Products.ToArray());
        return Task.FromResult(Result.Success(response));
    }

    public void Dispose()
    {
        _log.LogInformation("Dispose '{0}', uid = '{1}'", nameof(GetProductsService), _uid);
    }
}