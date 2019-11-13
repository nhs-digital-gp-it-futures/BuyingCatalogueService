using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Contracts.Persistence
{
    /// <summary>
    /// Defines a data contract representing the functionality for the persistence layer specific to the solution detail domain.
    /// </summary>
    public interface ISolutionDetailRepository
    {
        /// <summary>
        /// Updates the summary details of the solution.
        /// </summary>
        /// <param name="updateSolutionSummaryRequest">The updated details of a solution summary to save to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to save the specified updateSolutionRequest to the data store.</returns>
        Task UpdateSummaryAsync(IUpdateSolutionSummaryRequest updateSolutionSummaryRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the details of the solution.
        /// </summary>
        /// <param name="updateSolutionFeaturesRequest">The updated details of solution features to save to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task is cancelled.</param>
        /// <returns>A task representing an operation to save the specified solution to the data store.</returns>
        Task UpdateFeaturesAsync(IUpdateSolutionFeaturesRequest updateSolutionFeaturesRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the details of the solution.
        /// </summary>
        /// <param name="updateSolutionClientApplicationRequest">The updated details of solution client application to save to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task is cancelled.</param>
        /// <returns>A task representing an operation to save the specified solution to the data store.</returns>
        Task UpdateClientApplicationAsync(IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken);
    }
}
