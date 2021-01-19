using System;
using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.API.ViewModels
{
    public sealed class SolutionSummaryResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionSummaryResult"/> class.
        /// </summary>
        /// <param name="summary">The solution summary.</param>
        public SolutionSummaryResult(ISolutionSummary summary)
        {
            if (summary is null)
            {
                throw new ArgumentNullException(nameof(summary));
            }

            Id = summary.Id;
            Name = summary.Name;
            Summary = summary.Summary;
            IsFoundation = summary.IsFoundation;
            Supplier = summary.Supplier != null ? new SolutionSupplierResult(summary.Supplier) : null;
            Capabilities = summary.Capabilities.Select(cap => new SolutionCapabilityResult(cap)).ToList();
        }

        public string Id { get; }

        public string Name { get; }

        public string Summary { get; }

        public bool IsFoundation { get; }

        public SolutionSupplierResult Supplier { get; }

        public IEnumerable<SolutionCapabilityResult> Capabilities { get; }
    }
}
