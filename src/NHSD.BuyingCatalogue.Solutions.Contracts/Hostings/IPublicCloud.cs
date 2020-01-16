namespace NHSD.BuyingCatalogue.Solutions.Contracts.Hostings
{
    public interface IPublicCloud
    {
        string Summary { get; }

        string URL { get; }

        string ConnectivityRequired { get; }
    }
}
