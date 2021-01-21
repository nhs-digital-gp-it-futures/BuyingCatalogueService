using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    /// <summary>
    /// Represents the data access layer for the detail of a solution.
    /// </summary>
    public sealed class SolutionDetailRepository : ISolutionDetailRepository
    {
        private readonly IDbConnector dbConnector;

        public SolutionDetailRepository(IDbConnector dbConnector) => this.dbConnector = dbConnector;

        private const string UpdateTemplate = @"UPDATE s
   SET [Setters],
       s.LastUpdated = GETUTCDATE()
  FROM dbo.Solution AS s
       INNER JOIN dbo.CatalogueItem AS ci
               ON s.Id = ci.CatalogueItemId
 WHERE s.Id = @solutionId;

IF @@ROWCOUNT = 0
    THROW 60000, 'Solution not found', 1;";

        private const string GetClientApplicationBySolutionIdSql = @"SELECT Id, ClientApplication
  FROM dbo.Solution
 WHERE Id = @solutionId;";

        private const string GetHostingBySolutionIdSql = @"SELECT Id, Hosting
  FROM dbo.Solution
WHERE Id = @solutionId;";

        private const string GetRoadMapBySolutionIdSql = @"SELECT Id, RoadMap AS Summary
  FROM dbo.Solution
 WHERE Id = @solutionId;";

        private const string GetIntegrationsBySolutionIdSql = @"SELECT Id, IntegrationsUrl
  FROM dbo.Solution
 WHERE Id = @solutionId;";

        private const string GetImplementationTimescalesBySolutionIdSql = @"SELECT Id, ImplementationDetail AS [Description]
  FROM dbo.Solution
 WHERE Id = @solutionId;";

        /// <summary>
        /// Updates the summary details of the solution.
        /// </summary>
        /// <param name="updateSolutionSummaryRequest">The updated details of a solution to save to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to save the specified updateSolutionRequest to the data store.</returns>
        public async Task UpdateSummaryAsync(IUpdateSolutionSummaryRequest updateSolutionSummaryRequest, CancellationToken cancellationToken)
        {
            if (updateSolutionSummaryRequest is null)
            {
                throw new ArgumentNullException(nameof(updateSolutionSummaryRequest));
            }

            const string setters =
                @"s.FullDescription = @description,
                  s.Summary = @summary,
                  s.AboutUrl = @aboutUrl";

            await dbConnector.ExecuteAsync(
                UpdateTemplate.Replace("[Setters]", setters, StringComparison.InvariantCulture),
                cancellationToken,
                updateSolutionSummaryRequest);
        }

        /// <summary>
        /// Updates or inserts the features of the solution.
        /// </summary>
        /// <param name="updateSolutionFeaturesRequest">The updated features of a solution to save to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to save the specified updateSolutionRequest to the data store.</returns>
        public async Task UpdateFeaturesAsync(IUpdateSolutionFeaturesRequest updateSolutionFeaturesRequest, CancellationToken cancellationToken)
        {
            if (updateSolutionFeaturesRequest is null)
            {
                throw new ArgumentNullException(nameof(updateSolutionFeaturesRequest));
            }

            await dbConnector.ExecuteAsync(
                UpdateTemplate.Replace("[Setters]", @"s.Features = @features", StringComparison.InvariantCulture),
                cancellationToken,
                updateSolutionFeaturesRequest);
        }

        /// <summary>
        /// Adds or updates the client application details of a solution.
        /// </summary>
        /// <param name="updateSolutionClientApplicationRequest">The updated client application details of a solution to commit to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to save the specified <paramref name="updateSolutionClientApplicationRequest"/> details to the data store.</returns>
        public async Task UpdateClientApplicationAsync(IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken)
        {
            if (updateSolutionClientApplicationRequest is null)
            {
                throw new ArgumentNullException(nameof(updateSolutionClientApplicationRequest));
            }

            await dbConnector.ExecuteAsync(
                UpdateTemplate.Replace("[Setters]", @"s.ClientApplication = @clientApplication", StringComparison.InvariantCulture),
                cancellationToken,
                updateSolutionClientApplicationRequest);
        }

        public async Task<IClientApplicationResult> GetClientApplicationBySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
            => (await dbConnector.QueryAsync<ClientApplicationResult>(GetClientApplicationBySolutionIdSql, cancellationToken, new { solutionId })).SingleOrDefault();

        /// <summary>
        /// Adds or updates the hosting details of a solution.
        /// </summary>
        /// <param name="updateSolutionHostingRequest">The updated hosting details of a solution to commit to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to save the specified <paramref name="updateSolutionHostingRequest"/> details to the data store.</returns>
        public async Task UpdateHostingAsync(IUpdateSolutionHostingRequest updateSolutionHostingRequest, CancellationToken cancellationToken)
        {
            if (updateSolutionHostingRequest is null)
            {
                throw new ArgumentNullException(nameof(updateSolutionHostingRequest));
            }

            await dbConnector.ExecuteAsync(
                UpdateTemplate.Replace("[Setters]", @"s.Hosting = @hosting", StringComparison.InvariantCulture),
                cancellationToken,
                updateSolutionHostingRequest);
        }

        public async Task<IHostingResult> GetHostingBySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
            => (await dbConnector.QueryAsync<HostingResult>(GetHostingBySolutionIdSql, cancellationToken, new { solutionId })).SingleOrDefault();

        public async Task UpdateRoadMapAsync(IUpdateRoadMapRequest updateRoadMapRequest, CancellationToken cancellationToken)
        {
            if (updateRoadMapRequest is null)
            {
                throw new ArgumentNullException(nameof(updateRoadMapRequest));
            }

            await dbConnector.ExecuteAsync(
                UpdateTemplate.Replace("[Setters]", @"s.RoadMap = @description", StringComparison.InvariantCulture),
                cancellationToken,
                updateRoadMapRequest);
        }

        public async Task<IRoadMapResult> GetRoadMapBySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
            => (await dbConnector.QueryAsync<RoadMapResult>(GetRoadMapBySolutionIdSql, cancellationToken, new { solutionId })).SingleOrDefault();

        public async Task<IIntegrationsResult> GetIntegrationsBySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
            => (await dbConnector.QueryAsync<IntegrationsResult>(GetIntegrationsBySolutionIdSql, cancellationToken, new { solutionId })).SingleOrDefault();

        public async Task UpdateIntegrationsAsync(IUpdateIntegrationsRequest updateIntegrationsRequest, CancellationToken cancellationToken)
        {
            if (updateIntegrationsRequest is null)
            {
                throw new ArgumentNullException(nameof(updateIntegrationsRequest));
            }

            await dbConnector.ExecuteAsync(
                UpdateTemplate.Replace("[Setters]", @"s.IntegrationsUrl = @url", StringComparison.InvariantCulture),
                cancellationToken,
                updateIntegrationsRequest);
        }

        public async Task<IImplementationTimescalesResult> GetImplementationTimescalesBySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
            => (await dbConnector.QueryAsync<ImplementationTimescalesResult>(GetImplementationTimescalesBySolutionIdSql, cancellationToken, new { solutionId })).SingleOrDefault();

        public async Task UpdateImplementationTimescalesAsync(IUpdateImplementationTimescalesRequest updateImplementationTimescalesRequest, CancellationToken cancellationToken)
        {
            if (updateImplementationTimescalesRequest is null)
            {
                throw new ArgumentNullException(nameof(updateImplementationTimescalesRequest));
            }

            await dbConnector.ExecuteAsync(
                UpdateTemplate.Replace("[Setters]", @"s.ImplementationDetail = @description", StringComparison.InvariantCulture),
                cancellationToken,
                updateImplementationTimescalesRequest);
        }
    }
}
