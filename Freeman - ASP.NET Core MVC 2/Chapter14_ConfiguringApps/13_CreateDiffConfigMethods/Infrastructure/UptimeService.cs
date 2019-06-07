using System.Diagnostics;

namespace CreateDiffConfigMethods.Infrastructure
{
    public class UptimeService
    {
        private readonly string _creatorName;
        private readonly Stopwatch _timer;

        public UptimeService(string creatorName)
        {
            _creatorName = creatorName;
            _timer = Stopwatch.StartNew();
        }

        public long Uptime => _timer.ElapsedMilliseconds;

        public string DisplayUptime
        {
            get
            {
                var message = $"{Uptime}ms \n" +
                              $"Uptime service created by {_creatorName}.";

                return message;
            }
        }
    }
}
