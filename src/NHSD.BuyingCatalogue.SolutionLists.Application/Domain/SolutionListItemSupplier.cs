using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Domain
{
    internal sealed class SolutionListItemSupplier
    {
        /// <summary>
        /// Identifier of the supplier.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Name of the supplier.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionListItemSupplier"/> class.
        /// </summary>
        public SolutionListItemSupplier(ISolutionListResult item)
        {
            Id = item.SupplierId;
            Name = item.SupplierName;
        }
    }
}
