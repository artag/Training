using System.Diagnostics;

namespace ConfigLogging.Infrastructure
{
    public class UptimeService
    {
        private Stopwatch _timer;

        public UptimeService()
        {
            _timer = Stopwatch.StartNew();
        }

        public long Uptime => _timer.ElapsedMilliseconds;
    }
}
