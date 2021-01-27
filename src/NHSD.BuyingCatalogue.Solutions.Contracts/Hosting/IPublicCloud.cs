namespace NHSD.BuyingCatalogue.Solutions.Contracts.Hosting
{
    public interface IPublicCloud
    {
        string Summary { get; }

        string Link { get; }

        string RequiresHSCN { get; }
    }
}
