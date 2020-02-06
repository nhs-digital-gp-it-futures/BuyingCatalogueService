using Microsoft.Extensions.Configuration;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.API.Infrastructure
{
    internal class Settings : ISettings
    {
        private readonly IConfiguration _configuration;

        public Settings(IConfiguration configuration) => _configuration = configuration.ThrowIfNull();
        
        public string ConnectionString => _configuration.GetConnectionString("BuyingCatalogue");
        public string DocumentApiBaseUrl => _configuration["ApiClientSettings:DocumentApi:BaseUrl"];
        public string DocumentRoadMapIdentifier => _configuration["ApiClientSettings:DocumentApi:DocumentRoadMapIdentifier"];
        public string DocumentIntegrationIdentifier => _configuration["ApiClientSettings:DocumentApi:DocumentIntegrationIdentifier"];
    }
}
