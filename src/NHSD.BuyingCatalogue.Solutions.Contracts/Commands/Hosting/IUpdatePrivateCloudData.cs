namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hosting
{
    public interface IUpdatePrivateCloudData
    {
        string Summary { get; }

        string Link { get; }

        string HostingModel { get; }

        string RequiresHSCN { get; }
    }
}
