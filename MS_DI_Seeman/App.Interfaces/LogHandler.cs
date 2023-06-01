using Common;
using Microsoft.Extensions.Logging;

namespace App.Interfaces;

public class LogHandler<TRequest, TResponse>
    : IService<TRequest, TResponse>, IDisposable
{
    private readonly string _uid = Guid.NewGuid().ToString("D").Substring(0, 8);
    private readonly IService<TRequest, TResponse> _origin;
    private readonly ILogger<LogHandler<TRequest, TResponse>> _log;

    public LogHandler(
        IService<TRequest, TResponse> origin,
        ILoggerFactory loggerFactory)
    {
        _origin = origin ?? throw new ArgumentNullException(nameof(origin));
        if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
        _log = loggerFactory.CreateLogger<LogHandler<TRequest, TResponse>>()
            ?? throw new ArgumentNullException("log");

        _log.LogInformation("Ctor '{0}', uid = '{1}'", nameof(LogHandler<TRequest, TResponse>), _uid);
    }

    public async Task<Result<TResponse>> Execute(TRequest request)
    {
        _log.LogInformation("--- Begin execution request '{0}'", request!.GetType().Name);
        var result = await _origin.Execute(request);
        _log.LogInformation("--- End request '{0}'", request!.GetType().Name);
        return result;
    }

    public void Dispose()
    {
        _log.LogInformation("Dispose '{0}', uid = '{1}'", nameof(LogHandler<TRequest, TResponse>), _uid);
    }
}