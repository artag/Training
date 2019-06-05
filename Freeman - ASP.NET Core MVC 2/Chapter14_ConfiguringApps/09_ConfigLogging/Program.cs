using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace ConfigLogging
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
                   .ConfigureLogging(
                       (hostingContext, logging) =>
                       {
                           logging.AddConfiguration(
                               hostingContext.Configuration.GetSection("Logging"));
                           logging.AddConsole();
                           logging.AddDebug();
                       })
                   .UseIISIntegration()
                   .UseStartup<Startup>()
                   .Build();
        }
    }
}
