using MonitorApp.Models;

namespace MonitorApp.Services;

public interface IMonitorPanel
{
    IReadOnlyCollection<Indication> GetAll();
    int Add(Indication indication);
}
