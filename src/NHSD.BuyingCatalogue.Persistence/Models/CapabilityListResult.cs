using System;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Persistence.Models
{
    internal sealed class CapabilityListResult : ICapabilityListResult
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsFoundation { get; set; }
    }
}
