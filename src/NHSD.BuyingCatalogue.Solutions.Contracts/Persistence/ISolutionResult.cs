using System;
using System.Collections.Generic;

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

        string RoadMap { get; }

        string IntegrationsUrl { get; }

        string ImplementationTimescales { get; }

        string ClientApplication { get; }

        string Hosting { get; set; }

        bool IsFoundation { get; }

        DateTime SolutionDetailLastUpdated { get; }

        PublishedStatus PublishedStatus { get; }

        IEnumerable<ISolutionFrameworkListResult> Frameworks { get; set; }
    }
}
