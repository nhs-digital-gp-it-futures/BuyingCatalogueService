namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings
{
    public interface IUpdateOnPremiseData
    {
        string Summary { get; }

        string Link { get; }

        string HostingModel { get; }

        string RequiresHSCN { get; }
    }
}
