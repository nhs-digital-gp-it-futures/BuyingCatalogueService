using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions
{
    /// <summary>
    /// Represents the details of a supplier specific for the view.
    /// </summary>
    internal sealed class SolutionSupplierDto : ISolutionSupplier
    {
        /// <summary>
        /// Gets or sets the ID of the supplier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of supplier.
        /// </summary>
        public string Name { get; set; }
    }
}
