namespace NHSD.BuyingCatalogue.Contracts.Infrastructure
{
    public interface ISettings
    {
        string ConnectionString { get; }

        string DocumentApiBaseUrl { get; }
    }
}
