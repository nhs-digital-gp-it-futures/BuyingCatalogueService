namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IUpdateIntegrationsRequest
    {
        string SolutionId { get; }

        string Url { get; }
    }
}
