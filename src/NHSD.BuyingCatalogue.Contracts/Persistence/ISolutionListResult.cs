using System;

namespace NHSD.BuyingCatalogue.Contracts.Persistence
{
    public interface ISolutionListResult
    {
        string SolutionId { get; }

        string SolutionName { get; }

        string SolutionSummary { get; }

        string OrganisationId { get; }

        string OrganisationName { get; }

        Guid CapabilityId { get; }

        string CapabilityName { get; }

        string CapabilityDescription { get; }
    }
}
