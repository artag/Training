using Common;
using Microsoft.Extensions.Logging;

namespace App.Interfaces;

public class ExceptionHandler<TRequest, TResponse, TException>
    : IService<TRequest, TResponse>, IDisposable
    where TException : Exception
{
    private readonly string _uid = Guid.NewGuid().ToString("D").Substring(0, 8);
    private readonly IService<TRequest, TResponse> _origin;
    private readonly ILogger<ExceptionHandler<TRequest, TResponse, TException>> _log;
    private readonly Func<TException, Task<Result<TResponse>>> _handler;

    public ExceptionHandler(
        IService<TRequest, TResponse> origin,
        ILoggerFactory loggerFactory,
        Func<TException, Task<Result<TResponse>>> handler)
    {
        _origin = origin ?? throw new ArgumentNullException(nameof(origin));
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
        _log = loggerFactory.CreateLogger<ExceptionHandler<TRequest, TResponse, TException>>()
            ?? throw new ArgumentNullException(nameof(loggerFactory));

        _log.LogInformation("Ctor '{0}', uid = '{1}'", nameof(ExceptionHandler<TRequest, TResponse, TException>), _uid);
    }

    public Task<Result<TResponse>> Execute(TRequest request)
    {
        _log.LogInformation("--- Exception handler");

        try
        {
            return _origin.Execute(request);
        }
        catch (TException ex)
        {
            return _handler(ex);
        }
    }

    public void Dispose()
    {
        _log.LogInformation("Dispose '{0}', uid = '{1}'", nameof(ExceptionHandler<TRequest, TResponse, TException>), _uid);
    }
}
