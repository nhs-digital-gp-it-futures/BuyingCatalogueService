using Microsoft.Extensions.Configuration;

namespace NHSD.BuyingCatalogue.Application.Infrastructure
{
    public static class Settings
    {
        public static string BuyingCatalogueConnectionString(this IConfiguration config)
        {
            return config["ConnectionStrings:BuyingCatalogue"];
        }
    }
}
