namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hosting
{
    public interface IUpdatePublicCloudData
    {
        string Summary { get; }

        string Link { get; }

        string RequiresHscn { get; }
    }
}
