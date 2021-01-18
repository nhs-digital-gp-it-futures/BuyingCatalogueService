using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Domain
{
    internal sealed class SolutionListItemSupplier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionListItemSupplier"/> class.
        /// </summary>
        /// <param name="item">An <see cref="ISolutionListResult"/> instance.</param>
        public SolutionListItemSupplier(ISolutionListResult item)
        {
            Id = item.SupplierId;
            Name = item.SupplierName;
        }

        /// <summary>
        /// Gets the ID of the supplier.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the name of the supplier.
        /// </summary>
        public string Name { get; }
    }
}
