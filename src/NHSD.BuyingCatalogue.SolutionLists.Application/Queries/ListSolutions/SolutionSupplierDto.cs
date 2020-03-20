using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions
{
    /// <summary>
    /// Represents the details of a supplier specific for the view.
    /// </summary>
    internal sealed class SolutionSupplierDto : ISolutionSupplier
    {
        /// <summary>
        /// Identifier of supplier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of supplier.
        /// </summary>
        public string Name { get; set; }
    }
}
