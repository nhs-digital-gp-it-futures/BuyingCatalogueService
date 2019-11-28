using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IMarketingContactRepository
    {

        /// <summary>
        /// Gets an <see cref="IMarketingContactResult"/> matching the specified ID.
        /// </summary>
        /// <param name="solutionId">The ID of the solution to retrieve contacts for</param>
        /// <param name="cancellationToken">A token to notify if the task is cancelled.</param>
        /// <returns>A task representing an operation to retrieve a <see cref="IMarketingContactResult"/> matching the specified ID.</returns>
        Task<IEnumerable<IMarketingContactResult>> BySolutionIdAsync(string solutionId, CancellationToken cancellationToken);

        /// <summary>
        /// Removes current contacts for the solution and replaces them with the new <see cref="IContact"/> list
        /// </summary>
        /// <param name="solutionId">The ID of the solution to update.</param>
        /// <param name="newContacts">The new contacts to insert.</param>
        /// <param name="cancellationToken">A token to notify if the task is cancelled.</param>
        /// <returns></returns>
        Task ReplaceContactsForSolution(string solutionId, IEnumerable<IContact> newContacts, CancellationToken cancellationToken);
    }
}
