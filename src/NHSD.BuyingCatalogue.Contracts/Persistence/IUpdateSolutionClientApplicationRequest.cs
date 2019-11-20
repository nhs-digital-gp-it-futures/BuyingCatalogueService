namespace NHSD.BuyingCatalogue.Contracts.Persistence
{
    public interface IUpdateSolutionClientApplicationRequest
    {
        string SolutionId { get; }

        string ClientApplication { get; }
    }
}
