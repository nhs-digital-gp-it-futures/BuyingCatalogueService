using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISupplierRepository
    {
        /// <summary>
        /// Retrieve the Suppliers for a solution
        /// </summary>
        /// <param name="solutionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task containing the supplier data</returns>
        Task<ISolutionSupplierResult> GetSupplierBySolutionIdAsync(string solutionId, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the Supplier for a solution
        /// </summary>
        /// <param name="updateSupplierRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task containing the supplier data</returns>
        Task UpdateSupplierAsync(IUpdateSupplierRequest updateSupplierRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a list of suppliers matching the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the supplier to search for.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An asynchronous task context containing a list of suppliers.</returns>
        Task<IEnumerable<ISupplierResult>> GetSuppliersByName(string name, CancellationToken cancellationToken);
    }
}
