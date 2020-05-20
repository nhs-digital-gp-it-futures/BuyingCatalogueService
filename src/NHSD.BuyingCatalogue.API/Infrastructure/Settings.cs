using System;
using Microsoft.Extensions.Configuration;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;

namespace NHSD.BuyingCatalogue.API.Infrastructure
{
    internal sealed class Settings : ISettings
    {
        private readonly IConfiguration _configuration;

        public Settings(IConfiguration configuration) =>
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        public string ConnectionString => _configuration.GetConnectionString("BuyingCatalogue");

        public string DocumentApiBaseUrl => _configuration["ApiClientSettings:DocumentApi:BaseUrl"];

        public string DocumentRoadMapIdentifier => _configuration["ApiClientSettings:DocumentApi:DocumentRoadMapIdentifier"];

        public string DocumentIntegrationIdentifier => _configuration["ApiClientSettings:DocumentApi:DocumentIntegrationIdentifier"];

        public string DocumentSolutionIdentifier => _configuration["ApiClientSettings:DocumentApi:DocumentSolutionIdentifier"];
    }
}
