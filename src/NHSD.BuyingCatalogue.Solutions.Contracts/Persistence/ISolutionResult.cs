namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISolutionResult
    {
        string Id { get; }

        string Name { get; }

        string LastUpdated { get; }

        string Summary { get; }

        string Description { get; }

        string AboutUrl { get; }

        string Features { get; }

        string ClientApplication { get; }

        string OrganisationName { get; }

        bool IsFoundation { get; }

        PublishedStatus PublishedStatus { get; }
    }
}
