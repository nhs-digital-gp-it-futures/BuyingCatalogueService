namespace NHSD.BuyingCatalogue.Solutions.Contracts.Hostings
{
    public interface IPublicCloud
    {
        string Summary { get; }

        string Link { get; }

        string RequiresHSCN { get; }
    }
}
