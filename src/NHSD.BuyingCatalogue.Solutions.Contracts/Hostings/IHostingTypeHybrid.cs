namespace NHSD.BuyingCatalogue.Solutions.Contracts.Hostings
{
    public interface IHostingTypeHybrid
    {
        string Summary { get; }

        string Url { get; }

        string HostingModel { get; }

        string ConnectivityRequired { get; }
    }
}
