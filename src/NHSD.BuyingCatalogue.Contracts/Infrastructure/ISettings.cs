namespace NHSD.BuyingCatalogue.Contracts.Infrastructure
{
    public interface ISettings
    {
        string ConnectionString { get; }

        string DocumentApiBaseUrl { get; }

        string DocumentRoadMapIdentifier { get; }

        string DocumentIntegrationIdentifier { get; }
    }
}
