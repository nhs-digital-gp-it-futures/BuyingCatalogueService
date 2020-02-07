using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IUpdateCapabilityRequest
    {
        string SolutionId { get; }

        IEnumerable<string> NewCapabilitiesReference { get; }
    }
}
