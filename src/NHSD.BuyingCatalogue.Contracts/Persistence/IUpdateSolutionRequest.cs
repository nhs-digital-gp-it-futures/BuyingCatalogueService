namespace NHSD.BuyingCatalogue.Contracts.Persistence
{
    public interface IUpdateSolutionRequest
    {
        string Id { get; }

        string Summary { get; }

        string Description { get; }

        string AboutUrl { get; }

        string Features { get; }
    }
}
