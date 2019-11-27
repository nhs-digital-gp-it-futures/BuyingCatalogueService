namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IUpdateSolutionClientApplicationRequest
    {
        string SolutionId { get; }

        string ClientApplication { get; }
    }
}
