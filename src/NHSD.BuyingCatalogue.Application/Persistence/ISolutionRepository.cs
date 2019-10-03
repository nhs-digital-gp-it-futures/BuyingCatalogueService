using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    /// <summary>
    /// Defines a data contract representing the functionality for the persistence layer specific to the <see cref="Solution"/> domain.
    /// </summary>
    public interface ISolutionRepository
    {
        /// <summary>
        /// Gets a list of <see cref="Solution"/> objects.
        /// </summary>
        /// <returns>A list of <see cref="Solution"/> objects.</returns>
        Task<IEnumerable<Solution>> ListAsync(ISet<Guid> capabilityIdList, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a <see cref="Solution"/> matching the specified ID.
        /// </summary>
        /// <param name="id">The ID of the solution to look up.</param>
        /// <param name="cancellationToken">A token to nofity if the task is cancelled.</param>
        /// <returns>A task representing an operation to retrieve a <see cref="Solution"/> matching the specified ID.</returns>
        Task<Solution> ByIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the details of the solution.
        /// </summary>
        /// <param name="solution">The updated details of a solution to save to the data store.</param>
        /// <param name="cancellationToken">A token to nofity if the task is cancelled.</param>
        /// <returns>A task representing an operation to save the specified solution to the data store.</returns>
        Task UpdateAsync(Solution solution, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the supplier status of the specified solution in the data store.
        /// </summary>
        /// <param name="solution">The solution to update.</param>
        /// <param name="supplierStatus">The supplier status.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to update the supplier status of the specified solution in the data store.</returns>
        Task UpdateSupplierStatusAsync(Solution solution, SupplierStatus supplierStatus, CancellationToken cancellationToken);
    }
}
