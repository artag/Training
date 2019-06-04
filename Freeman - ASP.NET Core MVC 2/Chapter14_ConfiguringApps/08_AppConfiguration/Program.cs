using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AppConfiguration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return new WebHostBuilder()
                   .UseKestrel()
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .ConfigureAppConfiguration(
                       (hostingContext, config) =>
                       {
                           config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                           config.AddEnvironmentVariables();
                           if (args != null)
                           {
                               config.AddCommandLine(args);
                           }
                       })
                   .UseIISIntegration()
                   .UseStartup<Startup>()
                   .Build();
        }
    }
}
