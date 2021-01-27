namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hosting
{
    public interface IUpdateOnPremiseData
    {
        string Summary { get; }

        string Link { get; }

        string HostingModel { get; }

        string RequiresHscn { get; }
    }
}
