using System.Diagnostics.CodeAnalysis;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.SolutionLists.Persistence.Models
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by Dapper")]
    internal sealed class SolutionListResult : ISolutionListResult
    {
        public string SolutionId { get; init; }

        public string SolutionName { get; init; }

        public string SolutionSummary { get; init; }

        public string SupplierId { get; init; }

        public string SupplierName { get; init; }

        public string CapabilityReference { get; init; }

        public string CapabilityName { get; init; }

        public string CapabilityDescription { get; init; }

        public bool IsFoundation { get; init; }

        public string FrameworkId { get; set; }
    }
}
