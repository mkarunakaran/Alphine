using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Sumday.Service.ShareHolder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
             .ConfigureLogging((context, logging) =>
             {
                 logging.ClearProviders();
                 var loggingSection = context.Configuration.GetSection("Logging");
                 logging.AddConfiguration(loggingSection);
                 if (context.HostingEnvironment.IsDevelopment())
                 {
                     logging.AddDebug();
                     logging.AddConsole();
                 }

                 logging.AddEventSourceLogger();
             })
             .ConfigureAppConfiguration((context, config) =>
             {
                 var env = context.HostingEnvironment;

                 // These paths enable us to keep the configuration files outside of the project root
                 var basePath = Directory.GetParent(env.ContentRootPath).FullName;

                 var webPath = Path.GetFileName(env.ContentRootPath);
                 if (env.IsDevelopment())
                 {
                     webPath = ".";
                 }

                 var configurationPath = Path.Combine(basePath, webPath, "Configuration");

                 config.SetBasePath(basePath)
                    .AddJsonFile($"{configurationPath}/appsettings.json", true, true)
                    .AddJsonFile($"{configurationPath}/appsettings.{env.EnvironmentName}.json", true)
                    .AddJsonFile($"{configurationPath}/transactioncodes.json", false, true);
                
                 config.AddApplicationInsightsSettings(env.IsDevelopment());

                 config.AddSecrets(configurationPath, env.IsDevelopment());
                 var plansPath = Path.Combine(configurationPath, "plans");
                 config.AddKeyPerFile(directoryPath: plansPath, optional: false);
                 var fundsPath = Path.Combine(configurationPath, "funds");
                 config.AddKeyPerFile(directoryPath: fundsPath, optional: false);
                 var routingPath = Path.Combine(configurationPath, "routing");
                 config.AddKeyPerFile(directoryPath: routingPath, optional: false);
             })
             .ConfigureWebHostDefaults(webBuilder =>
             {
                webBuilder.UseStartup<Startup>();
                webBuilder.ConfigureKestrel(options =>
                {
                    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(20);
                });
             });
    }
}
