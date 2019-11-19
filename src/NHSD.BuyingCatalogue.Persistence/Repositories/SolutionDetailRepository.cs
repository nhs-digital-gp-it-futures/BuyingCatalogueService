using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Models;

namespace NHSD.BuyingCatalogue.Persistence.Repositories
{
    /// <summary>
    /// Represents the data access layer for the detail of a solution.
    /// </summary>
    public sealed class SolutionDetailRepository : ISolutionDetailRepository
    {
        private readonly DbConnector _dbConnector;

        public SolutionDetailRepository(DbConnector dbConnector) => _dbConnector = dbConnector;

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

            const string updateSql = @"
                                UPDATE  SolutionDetail                                   
                                SET     SolutionDetail.FullDescription = @description,
                                        SolutionDetail.Summary = @summary,
                                        SolutionDetail.AboutUrl = @aboutUrl
                                FROM SolutionDetail
                                    INNER JOIN Solution
                                        ON solution.Id = SolutionDetail.SolutionId AND SolutionDetail.Id = Solution.SolutionDetailId
                                WHERE   Solution.Id = @solutionId
                                IF @@ROWCOUNT = 0
                                    THROW 60000, 'Solution or SolutionDetail not found', 1; ";

            await _dbConnector.ExecuteAsync(cancellationToken, updateSql, new { solutionId = updateSolutionSummaryRequest.Id, description = updateSolutionSummaryRequest.Description, summary = updateSolutionSummaryRequest.Summary, aboutUrl = updateSolutionSummaryRequest.AboutUrl });
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
                throw new System.ArgumentNullException(nameof(updateSolutionFeaturesRequest));
            }

            const string updateSql = @"
                                UPDATE  SolutionDetail                                   
                                SET     SolutionDetail.Features = @features
                                FROM SolutionDetail
                                    INNER JOIN Solution
                                        ON solution.Id = SolutionDetail.SolutionId AND SolutionDetail.Id = Solution.SolutionDetailId
                                WHERE   Solution.Id = @solutionId
                                IF @@ROWCOUNT = 0
                                    THROW 60000, 'Solution or SolutionDetail not found', 1; ";

            await _dbConnector.ExecuteAsync(cancellationToken, updateSql, new { solutionId = updateSolutionFeaturesRequest.Id, features = updateSolutionFeaturesRequest.Features });
        }

        /// <summary>
        /// Adds or updates the client application details of a solution.
        /// </summary>
        /// <param name="updateSolutionClientApplicationRequest">The updated client application details of a solution to commit to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to save the specified <paramref name="updateSolutionClientApplicationRequest"/> details to the data store.</returns>
        public async Task UpdateClientApplicationAsync(IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest,
            CancellationToken cancellationToken)
        {
            if (updateSolutionClientApplicationRequest is null)
            {
                throw new ArgumentNullException(nameof(updateSolutionClientApplicationRequest));
            }

            const string updateSql = @"
                                UPDATE  SolutionDetail                                   
                                SET     SolutionDetail.ClientApplication = @clientApplication
                                FROM SolutionDetail
                                    INNER JOIN Solution
                                        ON solution.Id = SolutionDetail.SolutionId AND SolutionDetail.Id = Solution.SolutionDetailId
                                WHERE   Solution.Id = @solutionId
                                IF @@ROWCOUNT = 0
                                    THROW 60000, 'Solution or SolutionDetail not found', 1; ";
                
            await _dbConnector.ExecuteAsync(cancellationToken, updateSql, new { solutionId = updateSolutionClientApplicationRequest.Id, clientApplication = updateSolutionClientApplicationRequest.ClientApplication });
        }

        public async Task<IClientApplicationResult> GetClientApplicationBySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
        {
            const string sql = @"SELECT
                                    Solution.Id
                                    ,SolutionDetail.ClientApplication as ClientApplication
                                 FROM   Solution
                                        LEFT JOIN SolutionDetail ON Solution.Id = SolutionDetail.SolutionId AND SolutionDetail.Id = Solution.SolutionDetailId
                                 WHERE  Solution.Id = @solutionId";
            var result = await _dbConnector.QueryAsync<ClientApplicationResult>(cancellationToken, sql, new {solutionId});
            return result.SingleOrDefault();
        }
    }
}
