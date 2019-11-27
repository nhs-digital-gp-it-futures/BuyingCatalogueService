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
    }
}
