namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IUpdateSolutionFeaturesRequest
    {
        string SolutionId { get; }

        string Features { get; }
    }
}
