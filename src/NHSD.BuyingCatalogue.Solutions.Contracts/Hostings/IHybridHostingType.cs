namespace NHSD.BuyingCatalogue.Solutions.Contracts.Hostings
{
    public interface IHybridHostingType
    {
        string Summary { get; }

        string Url { get; }

        string HostingModel { get; }

        string ConnectivityRequired { get; }
    }
}
