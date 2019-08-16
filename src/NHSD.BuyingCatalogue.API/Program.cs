using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace NHSD.BuyingCatalogue.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		/// <summary>
		/// Creates the web host builder.
		/// </summary>
		/// <param name="args">A set of the application arguments.</param>
		/// <returns>An instance of the <see cref="IWebHostBuilder"/>.</returns>
		public static IWebHostBuilder CreateWebHostBuilder(string[] args)
		{
			IConfigurationRoot configurationRoot = GetConfiguration();

			return WebHost.CreateDefaultBuilder(args)
				.UseConfiguration(configurationRoot)
				.UseStartup<Startup>();
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
				.Build();
		}
	}
}
