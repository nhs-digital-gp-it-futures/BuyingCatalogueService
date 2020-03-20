using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.API.ViewModels
{
    public sealed class SolutionSummaryResult
    {
        public string Id { get; }

        public string Name { get; }

        public string Summary { get; }

        public bool IsFoundation { get; }

        public SolutionSupplierResult Supplier { get; }

        public IEnumerable<SolutionCapabilityResult> Capabilities { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionSummaryResult"/> class.
        /// </summary>
        public SolutionSummaryResult(ISolutionSummary summary)
        {
            Id = summary?.Id;
            Name = summary?.Name;
            Summary = summary?.Summary;
            IsFoundation = summary.ThrowIfNull(nameof(IsFoundation)).IsFoundation;
            Supplier = summary?.Supplier != null ? new SolutionSupplierResult(summary?.Supplier) : null;
            Capabilities = summary?.Capabilities.Select(cap => new SolutionCapabilityResult(cap)).ToList();
        }
    }
}
