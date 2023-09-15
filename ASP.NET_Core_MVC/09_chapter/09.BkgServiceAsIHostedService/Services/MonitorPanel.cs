using MonitorApp.Models;

namespace MonitorApp.Services;

public class MonitorPanel : IMonitorPanel
{
    private readonly List<Indication> _rows = new List<Indication>();

    public IReadOnlyCollection<Indication> GetAll()
    {
        return _rows;
    }

    public int Add(Indication indication)
    {
        if (_rows.Any())
        {
            indication.Id = _rows.Max(p => p.Id) + 1;
        }
        else
        {
            indication.Id = 1;
        }

        _rows.Add(indication);
        return indication.Id;
    }
}
