using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetfieldDeviceSample.Models;

namespace NetfieldDeviceSample
{
    class Program
    {
        public static void Main(string[] args)
        {
            var builder = new HostBuilder()
                          .ConfigureAppConfiguration((hostingContext, config) =>
              {
                  var env = hostingContext.HostingEnvironment;
                  config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                  config.AddEnvironmentVariables();

                  if (args != null)
                  {
                      config.AddCommandLine(args);
                  }
              })

              .ConfigureServices((hostContext, services) =>
              {
                  services.AddOptions();
                  services.Configure<MqttConfig>(hostContext.Configuration.GetSection("MqttConfig"));
                  services.AddTransient<IHostedService, Application>();
              })
              .ConfigureLogging((hostingContext, logging) =>
              {
                  logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                  logging.AddConsole();
              })
              .Build();
            builder.Run();
        }
    }
}
