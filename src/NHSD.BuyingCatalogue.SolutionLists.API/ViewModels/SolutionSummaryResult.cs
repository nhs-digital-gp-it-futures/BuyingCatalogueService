using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.API.ViewModels
{
    public sealed class SolutionSummaryResult : ISolutionSummary
    {
        public string Id { get; }

        public string Name { get; }

        public string Summary { get; }

        public bool IsFoundation { get; }

        [JsonProperty("supplier")]
        public SolutionSupplierResult SupplierResult { get; }

        [JsonIgnore]
        public ISolutionSupplier Supplier { get => SupplierResult; }

        [JsonProperty("capabilities")]
        public IEnumerable<SolutionCapabilityResult> CapabilitiesResult { get; }

        [JsonIgnore]
        public IEnumerable<ISolutionCapability> Capabilities { get => CapabilitiesResult; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionSummaryResult"/> class.
        /// </summary>
        public SolutionSummaryResult(ISolutionSummary summary)
        {
            Id = summary?.Id;
            Name = summary?.Name;
            Summary = summary?.Summary;
            IsFoundation = summary.ThrowIfNull(nameof(IsFoundation)).IsFoundation;
            SupplierResult = new SolutionSupplierResult(summary?.Supplier);
            CapabilitiesResult = summary?.Capabilities.Select(cap => new SolutionCapabilityResult(cap)).ToList();
        }
    }
}
