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
                        SolutionDetail.AboutUrl = @aboutUrl"), cancellationToken,updateSolutionSummaryRequest.ThrowIfNull(nameof(updateSolutionSummaryRequest))).ConfigureAwait(false);

        /// <summary>
        /// Updates or inserts the features of the solution.
        /// </summary>
        /// <param name="updateSolutionFeaturesRequest">The updated features of a solution to save to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to save the specified updateSolutionRequest to the data store.</returns>
        public async Task UpdateFeaturesAsync(IUpdateSolutionFeaturesRequest updateSolutionFeaturesRequest, CancellationToken cancellationToken)
            => await _dbConnector.ExecuteAsync(updateTemplate.Replace("[Setters]",
                    @"SolutionDetail.Features = @features"),
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
                    @"SolutionDetail.ClientApplication = @clientApplication"),
                cancellationToken,
                updateSolutionClientApplicationRequest.ThrowIfNull(nameof(updateSolutionClientApplicationRequest))).ConfigureAwait(false);

        public async Task<IClientApplicationResult> GetClientApplicationBySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
            => (await _dbConnector.QueryAsync<ClientApplicationResult>(getClientApplicationBySolutionIdSql, cancellationToken,new {solutionId}).ConfigureAwait(false)).SingleOrDefault();
    }
}
