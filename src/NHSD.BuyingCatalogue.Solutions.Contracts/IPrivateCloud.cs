namespace NHSD.BuyingCatalogue.Solutions.Contracts
{
    public interface IPrivateCloud
    {
        string Summary { get; }
        string Link { get; }
        string HostingModel { get; }
        string RequiresHSCN { get; }
    }
}
