namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IUpdateSolutionHostingRequest
    {
        string SolutionId { get; }

        string Hosting { get; }
    }
}
