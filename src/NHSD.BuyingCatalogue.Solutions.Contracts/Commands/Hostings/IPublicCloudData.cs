namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings
{
    public interface IPublicCloudData
    {
        string Summary { get; }

        string URL { get; }

        string ConnectivityRequired { get; }
    }
}
