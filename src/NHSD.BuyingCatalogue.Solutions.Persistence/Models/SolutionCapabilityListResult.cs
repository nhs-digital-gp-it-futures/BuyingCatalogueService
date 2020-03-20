using System;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    internal sealed class SolutionCapabilityListResult : ISolutionCapabilityListResult
    {
        public Guid CapabilityId { get; set; }

        public string CapabilityName { get; set; }

        public string CapabilityDescription { get; set; }

        public string CapabilityVersion { get; set; }
        public string CapabilitySourceUrl { get; set; }
    }
}
