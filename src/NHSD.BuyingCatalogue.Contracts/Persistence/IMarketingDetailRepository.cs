using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Contracts.Persistence
{
    /// <summary>
    /// Defines a data contract representing the functionality for the persistence layer specific to the Marketing detail domain.
    /// </summary>
    public interface IMarketingDetailRepository
    {
        /// <summary>
        /// Updates the details of the solution.
        /// </summary>
        /// <param name="updateSolutionFeaturesRequest">The updated details of solution features to save to the data store.</param>
        /// <param name="cancellationToken">A token to nofity if the task is cancelled.</param>
        /// <returns>A task representing an operation to save the specified solution to the data store.</returns>
        Task UpdateFeaturesAsync(IUpdateSolutionFeaturesRequest updateSolutionFeaturesRequest, CancellationToken cancellationToken);
    }
}
