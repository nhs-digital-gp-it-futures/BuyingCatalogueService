using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISupplierRepository
    {
        /// <summary>
        /// Retrieves the supplier matching the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the supplier to return.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An asynchronous task context containing the requested supplier.</returns>
        Task<ISupplierResult> GetSupplierById(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieve the Suppliers for a solution.
        /// </summary>
        /// <param name="solutionId">The ID of the solution.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the
        /// task to complete.</param>
        /// <returns>A task containing the supplier data.</returns>
        Task<ISolutionSupplierResult> GetSupplierBySolutionIdAsync(string solutionId, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the Supplier for a solution.
        /// </summary>
        /// <param name="updateSupplierRequest">The ID of the solution.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the
        /// task to complete.</param>
        /// <returns>A task containing the supplier data.</returns>
        Task UpdateSupplierAsync(IUpdateSupplierRequest updateSupplierRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a list of suppliers matching the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the supplier to search for.</param>
        /// <param name="solutionPublicationStatus">The solution status to use to filter the returned suppliers.</param>
        /// <param name="catalogueItemType">The catalogue item type to use to filter the returned suppliers.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An asynchronous task context containing a list of suppliers.</returns>
        /// <remarks>When <paramref name="solutionPublicationStatus"/> is specified only suppliers that have one or more
        /// items matching the specified status and item type will be returned. When the status and item type parameters are
        /// null all suppliers matching the specified <paramref name="name"/> will be returned.</remarks>
        Task<IEnumerable<ISupplierResult>> GetSuppliersByNameAsync(
            string name,
            PublishedStatus? solutionPublicationStatus,
            CatalogueItemType? catalogueItemType,
            CancellationToken cancellationToken);
    }
}
