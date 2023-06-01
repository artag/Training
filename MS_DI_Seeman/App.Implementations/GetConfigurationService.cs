using App.Interfaces;
using Common;
using Microsoft.Extensions.Logging;

namespace App.Implementations;

public class GetConfigurationService :
    IService<GetConfigurationRequest, GetConfigurationResponse>,
    IDisposable
{
    private readonly string _uid = Guid.NewGuid().ToString("D").Substring(0, 8);
    private readonly ILogger<GetConfigurationService> _log;

    public GetConfigurationService(ILogger<GetConfigurationService> log)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _log.LogInformation("Ctor '{0}', uid = '{1}'", nameof(GetConfigurationService), _uid);
    }

    public Task<Result<GetConfigurationResponse>> Execute(GetConfigurationRequest request)
    {
        var response = new GetConfigurationResponse(new Configuration("sqlite.db"));
        var result = Result.Success(response);
        return Task.FromResult(result);
    }

    public void Dispose()
    {
        _log.LogInformation("Dispose '{0}', uid = '{1}'", nameof(GetConfigurationService), _uid);
    }
}
