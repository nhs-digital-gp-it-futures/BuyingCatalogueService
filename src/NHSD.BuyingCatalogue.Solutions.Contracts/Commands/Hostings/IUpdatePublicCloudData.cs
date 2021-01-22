namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings
{
    public interface IUpdatePublicCloudData
    {
        string Summary { get; }

        string Link { get; }

        string RequiresHSCN { get; }
    }
}
