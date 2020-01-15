using System.Collections.Generic;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Domain
{
    internal sealed class SolutionListItem
    {
        /// <summary>
        /// Unique ID of the entity.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Name of the solution, as displayed to a user.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Summary of the solution, as displayed to a user.
        /// </summary>
        public string Summary { get; }

        /// <summary>
        /// Is this a foundation solution?
        /// </summary>
        public bool IsFoundation { get; }

        /// <summary>
        /// Associated supplier.
        /// </summary>
        public SolutionListItemSupplier Supplier { get; }

        /// <summary>
        /// A list of capabilities associated with the solution.
        /// </summary>
        public HashSet<SolutionListItemCapability> Capabilities { get; }

        public SolutionListItem(ISolutionListResult item)
        {
            Id = item.SolutionId;
            Name = item.SolutionName;
            Summary = item.SolutionSummary;
            IsFoundation = item.IsFoundation;
            Supplier = new SolutionListItemSupplier(item);
            Capabilities = new HashSet<SolutionListItemCapability>();
        }
    }
}
