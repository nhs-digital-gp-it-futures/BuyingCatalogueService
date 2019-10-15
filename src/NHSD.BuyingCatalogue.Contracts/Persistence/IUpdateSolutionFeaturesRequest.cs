namespace NHSD.BuyingCatalogue.Contracts.Persistence
{
    public interface IUpdateSolutionFeaturesRequest
    {
        string Id { get; }

        string Features { get; }
    }
}
