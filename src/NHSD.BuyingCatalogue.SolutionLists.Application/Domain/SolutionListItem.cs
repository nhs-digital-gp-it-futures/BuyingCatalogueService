using System.Collections.Generic;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Domain
{
    internal sealed class SolutionListItem
    {
        public SolutionListItem(ISolutionListResult item)
        {
            Id = item.SolutionId;
            Name = item.SolutionName;
            Summary = item.SolutionSummary;
            IsFoundation = item.IsFoundation;
            Supplier = new SolutionListItemSupplier(item);
            Capabilities = new HashSet<SolutionListItemCapability>();
        }

        /// <summary>
        /// Gets the unique ID of the entity.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the name of the solution, as displayed to a user.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the summary of the solution, as displayed to a user.
        /// </summary>
        public string Summary { get; }

        /// <summary>
        /// Gets a value indicating whether this a foundation solution.
        /// </summary>
        public bool IsFoundation { get; }

        /// <summary>
        /// Gets the associated supplier.
        /// </summary>
        public SolutionListItemSupplier Supplier { get; }

        /// <summary>
        /// Gets a list of capabilities associated with the solution.
        /// </summary>
        public HashSet<SolutionListItemCapability> Capabilities { get; }
    }
}
