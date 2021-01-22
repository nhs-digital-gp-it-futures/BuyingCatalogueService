using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace NHSD.BuyingCatalogue.API
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
#if DEBUG
                .WriteTo.Debug()
#endif
                .WriteTo.Console()
                .CreateLogger();
            try
            {
                Log.Information("Starting host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Creates the host builder.
        /// </summary>
        /// <param name="args">A set of the application arguments.</param>
        /// <returns>An instance of the <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IConfigurationRoot configurationRoot = GetConfiguration();

            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseConfiguration(configurationRoot)
                    .UseStartup<Startup>();
            });
        }

        /// <summary>
        /// Gets the application configuration root.
        /// </summary>
        /// <returns>An instance of the <see cref="IConfigurationRoot"/>.</returns>
        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
