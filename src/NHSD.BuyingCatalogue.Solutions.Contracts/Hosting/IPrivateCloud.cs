namespace NHSD.BuyingCatalogue.Solutions.Contracts.Hosting
{
    public interface IPrivateCloud
    {
        string Summary { get; }

        string Link { get; }

        string HostingModel { get; }

        string RequiresHSCN { get; }
    }
}
