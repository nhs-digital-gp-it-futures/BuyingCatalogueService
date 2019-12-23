using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace NHSD.BuyingCatalogue.API
{
    public static class Program
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
				.AddUserSecrets(Assembly.GetExecutingAssembly())
				.AddEnvironmentVariables()
				.Build();
		}
	}
}
