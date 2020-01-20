namespace NHSD.BuyingCatalogue.Solutions.Contracts.Hostings
{
    public interface IOnPremise
    {
        string Summary { get; }
        string Link { get; }
        string HostingModel { get; }
        string RequiresHSCN { get; }
    }
}
