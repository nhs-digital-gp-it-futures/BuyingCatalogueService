using System;
using Microsoft.Extensions.Configuration;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;

namespace NHSD.BuyingCatalogue.API.Infrastructure
{
    internal sealed class Settings : ISettings
    {
        private readonly IConfiguration configuration;

        public Settings(IConfiguration configuration) =>
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        public string ConnectionString => configuration.GetConnectionString("BuyingCatalogue");

        public string DocumentApiBaseUrl => configuration["ApiClientSettings:DocumentApi:BaseUrl"];

        public string DocumentRoadMapIdentifier => configuration["ApiClientSettings:DocumentApi:DocumentRoadMapIdentifier"];

        public string DocumentIntegrationIdentifier => configuration["ApiClientSettings:DocumentApi:DocumentIntegrationIdentifier"];

        public string DocumentSolutionIdentifier => configuration["ApiClientSettings:DocumentApi:DocumentSolutionIdentifier"];
    }
}
