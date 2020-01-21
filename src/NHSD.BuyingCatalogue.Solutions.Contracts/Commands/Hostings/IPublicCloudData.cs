namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings
{
    public interface IPublicCloudData
    {
        string Summary { get; }

        string Link { get; }

        string RequiresHSCN { get; }
    }
}
