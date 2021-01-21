using System;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISolutionCapabilityListResult
    {
        Guid CapabilityId { get; }

        string CapabilityName { get; }

        string CapabilityDescription { get; }

        string CapabilityVersion { get; }

        string CapabilitySourceUrl { get; }
    }
}
