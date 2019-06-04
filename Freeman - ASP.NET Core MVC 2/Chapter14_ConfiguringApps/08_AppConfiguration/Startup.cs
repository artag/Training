using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppConfiguration
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Run(async (context) =>
            {
                var message = $"Readed Data from JSON file. 1st way: {ReadDataFirstWay()}\n" +
                              $"Readed Data from JSON file. 2nd way: {ReadDataSecondWay()}\n" +
                              $"Readed Data from JSON file. 3rd way: {ReadDataThirdWay()}\n" +
                              "\n" +
                              $"Readed first value from MySettings: {GetFirstValueFromMySettings()}\n" +
                              $"Readed second value from MySettings: {GetSecondValueFromMySettings()}";

                await context.Response.WriteAsync(message);
            });
        }

        private bool ReadDataFirstWay()
        {
            IConfigurationSection section = _configuration.GetSection("ShortCircuitMiddleware");
            bool? value = section?.GetValue<bool>("EnableBrowserShortCircuit");
            return value.Value;
        }

        private bool ReadDataSecondWay()
        {
            return (_configuration.GetSection("ShortCircuitMiddleware")
                                  ?.GetValue<bool>("EnableBrowserShortCircuit")).Value;
        }

        private string ReadDataThirdWay()
        {
            return _configuration["ShortCircuitMiddleware:EnableBrowserShortCircuit"];
        }

        private string GetFirstValueFromMySettings()
        {
            return _configuration.GetSection("MySettings:0").Value;
        }

        private string GetSecondValueFromMySettings()
        {
            return _configuration.GetSection("MySettings:1").Value;
        }
    }
}
