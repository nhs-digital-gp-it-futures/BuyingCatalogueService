namespace NHSD.BuyingCatalogue.Contracts.Persistence
{
    public interface IUpdateSolutionSummaryRequest
    {
        string Id { get; }

        string Summary { get; }

        string Description { get; }

        string AboutUrl { get; }
    }
}
