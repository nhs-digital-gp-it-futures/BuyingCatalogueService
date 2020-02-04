using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    /// <summary>
    /// Represents the data access layer for the detail of a solution.
    /// </summary>
    public sealed class SolutionDetailRepository : ISolutionDetailRepository
    {
        private readonly IDbConnector _dbConnector;

        public SolutionDetailRepository(IDbConnector dbConnector) => _dbConnector = dbConnector;

        private const string updateTemplate = @"
                                UPDATE  SolutionDetail
                                SET     [Setters],
								SolutionDetail.LastUpdated = GETDATE()
                                FROM SolutionDetail
                                    INNER JOIN Solution
                                        ON solution.Id = SolutionDetail.SolutionId AND SolutionDetail.Id = Solution.SolutionDetailId
                                WHERE   Solution.Id = @solutionId
                                IF @@ROWCOUNT = 0
                                    THROW 60000, 'Solution or SolutionDetail not found', 1; ";

        const string getClientApplicationBySolutionIdSql = @"SELECT
                                    Solution.Id
                                    ,SolutionDetail.ClientApplication as ClientApplication
                                 FROM   Solution
                                        LEFT JOIN SolutionDetail ON Solution.Id = SolutionDetail.SolutionId AND SolutionDetail.Id = Solution.SolutionDetailId
                                 WHERE  Solution.Id = @solutionId";

        const string getHostingBySolutionIdSql = @"SELECT
                                    Solution.Id
                                    ,SolutionDetail.Hosting as Hosting
                                 FROM   Solution
                                        LEFT JOIN SolutionDetail ON Solution.Id = SolutionDetail.SolutionId AND SolutionDetail.Id = Solution.SolutionDetailId
                                 WHERE  Solution.Id = @solutionId";

        const string GetRoadMapBySolutionIdSql = @"SELECT
                                    Solution.Id,
                                    SolutionDetail.RoadMap as Summary
                                 FROM   Solution
                                        LEFT JOIN SolutionDetail ON Solution.Id = SolutionDetail.SolutionId AND SolutionDetail.Id = Solution.SolutionDetailId
                                 WHERE  Solution.Id = @solutionId";

        const string GetIntegrationsBySolutionIdSql = @"SELECT
                                    Solution.Id,
                                    SolutionDetail.IntegrationsUrl as IntegrationsUrl
                                 FROM   Solution
                                        LEFT JOIN SolutionDetail ON Solution.Id = SolutionDetail.SolutionId AND SolutionDetail.Id = Solution.SolutionDetailId
                                 WHERE  Solution.Id = @solutionId";

        const string GetImplementationTimescalesBySolutionIdSql = @"SELECT
                                    Solution.Id,
                                    SolutionDetail.ImplementationDetail as Description
                                 FROM   Solution
                                        LEFT JOIN SolutionDetail ON Solution.Id = SolutionDetail.SolutionId AND SolutionDetail.Id = Solution.SolutionDetailId
                                 WHERE  Solution.Id = @solutionId";

        /// <summary>
        /// Updates the summary details of the solution.
        /// </summary>
        /// <param name="updateSolutionSummaryRequest">The updated details of a solution to save to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to save the specified updateSolutionRequest to the data store.</returns>
        public async Task UpdateSummaryAsync(IUpdateSolutionSummaryRequest updateSolutionSummaryRequest, CancellationToken cancellationToken)
            => await _dbConnector.ExecuteAsync(updateTemplate.Replace("[Setters]",
                        @"SolutionDetail.FullDescription = @description,
                        SolutionDetail.Summary = @summary,
                        SolutionDetail.AboutUrl = @aboutUrl", StringComparison.InvariantCulture), cancellationToken,updateSolutionSummaryRequest.ThrowIfNull(nameof(updateSolutionSummaryRequest))).ConfigureAwait(false);

        /// <summary>
        /// Updates or inserts the features of the solution.
        /// </summary>
        /// <param name="updateSolutionFeaturesRequest">The updated features of a solution to save to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to save the specified updateSolutionRequest to the data store.</returns>
        public async Task UpdateFeaturesAsync(IUpdateSolutionFeaturesRequest updateSolutionFeaturesRequest, CancellationToken cancellationToken)
            => await _dbConnector.ExecuteAsync(updateTemplate.Replace("[Setters]",
                    @"SolutionDetail.Features = @features", StringComparison.InvariantCulture),
                cancellationToken,
                updateSolutionFeaturesRequest.ThrowIfNull(nameof(updateSolutionFeaturesRequest))).ConfigureAwait(false);

        /// <summary>
        /// Adds or updates the client application details of a solution.
        /// </summary>
        /// <param name="updateSolutionClientApplicationRequest">The updated client application details of a solution to commit to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to save the specified <paramref name="updateSolutionClientApplicationRequest"/> details to the data store.</returns>
        public async Task UpdateClientApplicationAsync(IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken)
            => await _dbConnector.ExecuteAsync(updateTemplate.Replace("[Setters]",
                    @"SolutionDetail.ClientApplication = @clientApplication", StringComparison.InvariantCulture),
                cancellationToken,
                updateSolutionClientApplicationRequest.ThrowIfNull(nameof(updateSolutionClientApplicationRequest))).ConfigureAwait(false);

        public async Task<IClientApplicationResult> GetClientApplicationBySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
            => (await _dbConnector.QueryAsync<ClientApplicationResult>(getClientApplicationBySolutionIdSql, cancellationToken,new {solutionId}).ConfigureAwait(false)).SingleOrDefault();

        /// <summary>
        /// Adds or updates the hosting details of a solution.
        /// </summary>
        /// <param name="updateSolutionHostingRequest">The updated hosting details of a solution to commit to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to save the specified <paramref name="updateSolutionHostingRequest"/> details to the data store.</returns>
        public async Task UpdateHostingAsync(IUpdateSolutionHostingRequest updateSolutionHostingRequest, CancellationToken cancellationToken)
            => await _dbConnector.ExecuteAsync(updateTemplate.Replace("[Setters]",
                    @"SolutionDetail.Hosting = @hosting", StringComparison.InvariantCulture),
                cancellationToken,
                updateSolutionHostingRequest.ThrowIfNull(nameof(updateSolutionHostingRequest))).ConfigureAwait(false);

        public async Task<IHostingResult> GetHostingBySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
            => (await _dbConnector.QueryAsync<HostingResult>(getHostingBySolutionIdSql, cancellationToken, new { solutionId }).ConfigureAwait(false)).SingleOrDefault();

        public async Task UpdateRoadmapAsync(IUpdateRoadmapRequest updateRoadmapRequest, CancellationToken cancellationToken)
            => await _dbConnector.ExecuteAsync(updateTemplate.Replace("[Setters]",
                    @"SolutionDetail.RoadMap = @description", StringComparison.InvariantCulture),
                cancellationToken,
                updateRoadmapRequest.ThrowIfNull(nameof(updateRoadmapRequest))).ConfigureAwait(false);

        public async Task<IRoadMapResult> GetRoadMapBySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
            => (await _dbConnector.QueryAsync<RoadMapResult>(GetRoadMapBySolutionIdSql, cancellationToken, new { solutionId }).ConfigureAwait(false)).SingleOrDefault();

        public async Task<IIntegrationsResult> GetIntegrationsBySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
            => (await _dbConnector.QueryAsync<IntegrationsResult>(GetIntegrationsBySolutionIdSql, cancellationToken, new { solutionId }).ConfigureAwait(false)).SingleOrDefault();

        public async Task UpdateIntegrationsAsync(IUpdateIntegrationsRequest updateIntegrationsRequest, CancellationToken cancellationToken)
            => await _dbConnector.ExecuteAsync(updateTemplate.Replace("[Setters]",
                    @"SolutionDetail.IntegrationsUrl = @url", StringComparison.InvariantCulture),
                cancellationToken,
                updateIntegrationsRequest.ThrowIfNull(nameof(updateIntegrationsRequest))).ConfigureAwait(false);

        public async Task<IImplementationTimescalesResult> GetImplementationTimescalesBySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
            => (await _dbConnector.QueryAsync<ImplementationTimescalesResult>(GetImplementationTimescalesBySolutionIdSql, cancellationToken, new { solutionId }).ConfigureAwait(false)).SingleOrDefault();

    }
}
