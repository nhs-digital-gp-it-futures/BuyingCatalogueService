using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace NHSD.BuyingCatalogue.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

        /// <summary>
        /// Creates the host builder.
        /// </summary>
        /// <param name="args">A set of the application arguments.</param>
        /// <returns>An instance of the <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
		{
			IConfigurationRoot configurationRoot = GetConfiguration();

            foreach (var item in configurationRoot.AsEnumerable().OrderBy(item => item.Key))
            {
                Console.WriteLine($"Configuration Key='{item.Key}' Value='{item.Value}'");
            }

            return Host.CreateDefaultBuilder(args)
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
				.AddJsonFile("appsettings.json")
				.AddUserSecrets<Program>()
				.AddEnvironmentVariables()
				.Build();
		}
	}
}
