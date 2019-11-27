using System;

namespace NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence
{
    public interface ICapabilityListResult
    {
        Guid Id { get; }

        string Name { get; }

        bool IsFoundation { get; }
    }
}
