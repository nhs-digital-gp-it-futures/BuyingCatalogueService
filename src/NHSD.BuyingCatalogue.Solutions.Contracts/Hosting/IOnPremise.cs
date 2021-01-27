namespace NHSD.BuyingCatalogue.Solutions.Contracts.Hosting
{
    public interface IOnPremise
    {
        string Summary { get; }

        string Link { get; }

        string HostingModel { get; }

        string RequiresHscn { get; }
    }
}
