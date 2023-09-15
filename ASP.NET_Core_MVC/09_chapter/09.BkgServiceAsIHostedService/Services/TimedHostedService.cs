using System.Diagnostics;
using MonitorApp.Models;

namespace MonitorApp.Services;

/// <summary>
/// Фоновая задача, управляемая хостом.
/// </summary>
public class TimedHostedService : IHostedService, IDisposable
{
    private readonly IMonitorPanel _monitor;
    private Timer? _timer;

    public TimedHostedService(IMonitorPanel monitor)
    {
        _monitor = monitor;
    }

    // 1. Вызывается фреймворком до настройки конвейера обработки запросов.
    // 2. Вызывается при запуске сервера.
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Создание и запуск периодического таймера.
        _timer = new Timer(
            callback: DoWork,
            state: null,
            dueTime: TimeSpan.Zero,
            period: TimeSpan.FromSeconds(10));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Остановка таймера.
        _timer?.Change(
            dueTime: Timeout.Infinite,
            period: 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    private void DoWork(object? state)
    {
        var prc = Process.GetCurrentProcess();
        var mem = prc.WorkingSet64;     // Занятая процессом RAM, в байтах.
        var memMb = mem / 1024 / 1024;
        var now = DateTime.Now;
        var ind = new Indication { TimePoint = now, Memory = memMb };
        _monitor.Add(ind);
    }
}
