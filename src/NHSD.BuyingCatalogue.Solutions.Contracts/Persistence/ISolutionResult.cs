using System;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISolutionResult
    {
        string Id { get; }

        string Name { get; }

        DateTime LastUpdated { get; }

        string Summary { get; }

        string Description { get; }

        string AboutUrl { get; }

        string Features { get; }

        string ClientApplication { get; }

        string OrganisationName { get; }

        bool IsFoundation { get; }

        DateTime SolutionDetailLastUpdated { get; }
        
        PublishedStatus PublishedStatus { get; }
    }
}
