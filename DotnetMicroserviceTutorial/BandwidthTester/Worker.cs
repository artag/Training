using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpeedTest;
using SpeedTest.Models;

namespace BandwidthTester
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly SpeedTestClient _client;
        private readonly Settings _settings;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _client = new SpeedTestClient();
            _settings = _client.GetSettings();
        }

        private IEnumerable<Server> GetTestServers()
        {
            _logger.LogInformation("Get Test Servers");

            return _settings.Servers
                .OrderBy(server => server.Distance)
                .Take(5)
                .ForEach(srv => srv.Latency = _client.TestServerLatency(srv))
                .Take(2);
        }

        private double GetAverageDownloadSpeed(IEnumerable<Server> servers)
        {
            _logger.LogInformation("Testing Download Speed");

            return servers
                .Select(server => _client.TestDownloadSpeed(server, _settings.Download.ThreadsPerUrl))
                .Average();
        }

        private double GetAverageUploadSpeed(IEnumerable<Server> servers)
        {
            _logger.LogInformation("Testing Upload Speed");

            return servers
                .Select(server => _client.TestUploadSpeed(server, _settings.Download.ThreadsPerUrl))
                .Average();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Testing bandwidth start at {time}", DateTimeOffset.Now);

                var servers = GetTestServers();
                var avgDownloadSpeed = Math.Round(GetAverageDownloadSpeed(servers), digits: 0) / 1000;
                var avgUploadSpeed = Math.Round(GetAverageUploadSpeed(servers), digits: 0) / 1000;

                _logger.LogInformation("{LogMessage} at {time}: Download: {download} MBit/s, Upload: {upload} MBit/s",
                    "BandwidthTest", DateTimeOffset.Now, avgDownloadSpeed, avgUploadSpeed);

                await Task.Delay(new TimeSpan(hours: 1, minutes: 0, seconds: 0), stoppingToken);
            }
        }
    }
}
