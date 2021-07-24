using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CoronavirusApiService.ApiModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoronavirusApiService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ;
            while (!stoppingToken.IsCancellationRequested)
            {
                // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                using (var webClient = new WebClient())
                {
                    var json = webClient.DownloadString("https://coronavirus-tracker-api.herokuapp.com/v2/locations");
                    var data = JsonConvert.DeserializeObject<ApiData>(json);
                    _logger.LogInformation("{@Latest}", data.Latest);
                }
                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            }
        }
    }
}
