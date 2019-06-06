using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CreateDiffExternalConfigFiles
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
                           var env = hostingContext.HostingEnvironment;

                           var configPath1 = "appsettings.json";
                           var configPath2 = $"appsettings.{env.EnvironmentName}.json";

                           config.AddJsonFile(configPath1, optional: true, reloadOnChange: true)
                                 .AddJsonFile(configPath2, optional: true, reloadOnChange: true);

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
