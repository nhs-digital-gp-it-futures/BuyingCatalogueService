using Microsoft.Extensions.Configuration;

namespace NHSD.BuyingCatalogue.Application.Infrastructure
{
    /// <summary>
    /// Provides the settings for the application.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Gets the buying catalogue connection string.
        /// </summary>
        /// <param name="configuration">The configuration provider to retrieve the value of a setting.</param>
        /// <returns>The connection string for the Buying Catalogue.</returns>
        public static string BuyingCatalogueConnectionString(this IConfiguration configuration)
        {
            return configuration["ConnectionStrings:BuyingCatalogue"];
        }
    }
}
