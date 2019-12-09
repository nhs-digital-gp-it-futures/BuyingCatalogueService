using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    /// <summary>
    /// Defines a data contract representing the functionality for the persistence layer specific to the <see cref="ISolutionResult"/> object.
    /// </summary>
    public interface ISolutionRepository
    {
        /// <summary>
        /// Gets a <see cref="ISolutionResult"/> matching the specified ID.
        /// </summary>
        /// <param name="id">The ID of the solution to look up.</param>
        /// <param name="cancellationToken">A token to notify if the task is cancelled.</param>
        /// <returns>A task representing an operation to retrieve a <see cref="ISolutionListResult"/> matching the specified ID.</returns>
        Task<ISolutionResult> ByIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the supplier status of the specified solution in the data store.
        /// </summary>
        /// <param name="updateSolutionSupplierStatusRequest">The details of the solution supplier status update.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to update the supplier status of the specified solution in the data store.</returns>
        Task UpdateSupplierStatusAsync(IUpdateSolutionSupplierStatusRequest updateSolutionSupplierStatusRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Checks if the solution exists
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>True if it exists</returns>
        Task<bool> CheckExists(string id, CancellationToken cancellationToken);
    }
}
