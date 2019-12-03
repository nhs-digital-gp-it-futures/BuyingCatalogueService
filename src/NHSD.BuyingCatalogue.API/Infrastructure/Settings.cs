using Microsoft.Extensions.Configuration;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.API.Infrastructure
{
    internal class Settings : ISettings
    {
        private readonly IConfiguration _configuration;

        public Settings(IConfiguration configuration) => _configuration = configuration.ThrowIfNull();

        public string ConnectionString =>
            "Server=tcp:gpitfutures-sqlpri-dev.database.windows.net,1433;Initial Catalog=gpitfutures-db-dev;Persist Security Info=False;User ID=gpitfutureadmin;Password=6vygJRq9RhZy5knQw3WqkhPsPm77sujp;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"; //_configuration["ConnectionStrings:BuyingCatalogue"];
    }
}
