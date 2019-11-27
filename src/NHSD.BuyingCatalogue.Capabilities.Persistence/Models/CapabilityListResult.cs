using System;
using NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Capabilities.Persistence.Models
{
    internal sealed class CapabilityListResult : ICapabilityListResult
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsFoundation { get; set; }
    }
}
