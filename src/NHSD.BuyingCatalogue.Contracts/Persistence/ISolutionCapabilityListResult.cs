using System;
using System.Collections.Generic;
using System.Text;

namespace NHSD.BuyingCatalogue.Contracts.Persistence
{
    public interface ISolutionCapabilityListResult
    {
        Guid CapabilityId { get; }

        string CapabilityName { get; }

        string CapabilityDescription { get; }
    }
}
