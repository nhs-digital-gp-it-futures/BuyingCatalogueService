namespace NHSD.BuyingCatalogue.Contracts.Persistence
{
    public interface IUpdateSolutionFeaturesRequest
    {
        string SolutionId { get; }

        string Features { get; }
    }
}
