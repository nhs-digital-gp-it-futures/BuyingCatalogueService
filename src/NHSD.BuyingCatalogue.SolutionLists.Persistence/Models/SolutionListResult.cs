using System;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.SolutionLists.Persistence.Models
{
    internal sealed class SolutionListResult : ISolutionListResult
    {
        public string SolutionId { get; set; }

        public string SolutionName { get; set; }

        public string SolutionSummary { get; set; }

        public string SupplierId { get; set; }

        public string SupplierName { get; set; }

        public Guid CapabilityId { get; set; }

        public string CapabilityName { get; set; }

        public string CapabilityDescription { get; set; }

        public bool IsFoundation { get; set; }
    }
}
