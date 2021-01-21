using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
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

        /// <summary>
        /// Retrieve the client application details for a solution.
        /// </summary>
        /// <param name="solutionId">The ID of the solution.</param>
        /// <param name="cancellationToken">A token to notify if the task is cancelled.</param>
        /// <returns>A task containing the client application data.</returns>
        Task<IClientApplicationResult> GetClientApplicationBySolutionIdAsync(string solutionId, CancellationToken cancellationToken);

        /// <summary>
        /// Update the hosting details for a solution.
        /// </summary>
        /// <param name="updateSolutionHostingRequest">The updated details of solution hosting to save to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task is cancelled.</param>
        /// <returns>A task containing the hosting data.</returns>
        Task UpdateHostingAsync(IUpdateSolutionHostingRequest updateSolutionHostingRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieve the hosting details for a solution.
        /// </summary>
        /// <param name="solutionId">The ID of the solution.</param>
        /// <param name="cancellationToken">A token to notify if the task is cancelled.</param>
        /// <returns>A task containing the hosting data.</returns>
        Task<IHostingResult> GetHostingBySolutionIdAsync(string solutionId, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieve the road map details for a solution.
        /// </summary>
        /// <param name="solutionId">The ID of the solution.</param>
        /// <param name="cancellationToken">A token to notify if the task is cancelled.</param>
        /// <returns>A task containing the road map data.</returns>
        Task<IRoadMapResult> GetRoadMapBySolutionIdAsync(string solutionId, CancellationToken cancellationToken);

        /// <summary>
        /// Update the road map details for a solution.
        /// </summary>
        /// <param name="updateRoadMapRequest">The updated details of solution road map to save to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task is cancelled.</param>
        /// <returns>A task containing the road map data.</returns>
        Task UpdateRoadMapAsync(IUpdateRoadMapRequest updateRoadMapRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieve the integrations details for a solution.
        /// </summary>
        /// <param name="solutionId">The ID of the solution.</param>
        /// <param name="cancellationToken">A token to notify if the task is cancelled.</param>
        /// <returns>A task containing the integrations data.</returns>
        Task<IIntegrationsResult> GetIntegrationsBySolutionIdAsync(string solutionId, CancellationToken cancellationToken);

        /// <summary>
        /// Update the integrations details for a solution.
        /// </summary>
        /// <param name="updateIntegrationsRequest">The updated details of solution integrations to save to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task is cancelled.</param>
        /// <returns>A task containing the integrations data.</returns>
        Task UpdateIntegrationsAsync(IUpdateIntegrationsRequest updateIntegrationsRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieve the implementation timescales details for a solution.
        /// </summary>
        /// <param name="solutionId">The ID of the solution.</param>
        /// <param name="cancellationToken">A token to notify if the task is cancelled.</param>
        /// <returns>A task containing the implementation timescales data.</returns>
        Task<IImplementationTimescalesResult> GetImplementationTimescalesBySolutionIdAsync(string solutionId, CancellationToken cancellationToken);

        /// <summary>
        /// Update the implementation timescales details for a solution.
        /// </summary>
        /// <param name="updateImplementationTimescalesRequest">The updated details of solution implementation timescales to save to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task is cancelled.</param>
        /// <returns>A task containing the implementation timescales data.</returns>
        Task UpdateImplementationTimescalesAsync(
            IUpdateImplementationTimescalesRequest updateImplementationTimescalesRequest,
            CancellationToken cancellationToken);
    }
}
