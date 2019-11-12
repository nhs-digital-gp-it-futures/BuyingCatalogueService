using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;

namespace NHSD.BuyingCatalogue.Persistence.Repositories
{
    /// <summary>
    /// Represents the data access layer for the marketing data of a solution.
    /// </summary>
    public sealed class SolutionDetailRepository : ISolutionDetailRepository
    {
        /// <summary>
        /// Database connection factory to provide new connections.
        /// </summary>
        private IDbConnectionFactory DbConnectionFactory { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionRepository"/> class.
        /// </summary>
        public SolutionDetailRepository(IDbConnectionFactory dbConnectionFactory)
        {
            DbConnectionFactory = dbConnectionFactory;
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

            using (IDbConnection databaseConnection = await DbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
            {
                const string updateSql = @" IF EXISTS (SELECT 1 FROM Solution WHERE Id = @solutionId)
                                        MERGE SolutionDetail AS target  
                                        USING (SELECT @solutionId, @features) AS source (SolutionId, Features)
                                        ON (target.Id = source.SolutionDetailId AND target.PublishStatus nOT PUBLISHED)  
                                        WHEN MATCHED THEN
                                            UPDATE
                                            SET  Features = source.Features
                                        WHEN NOT MATCHED THEN
                                            INSERT (Id, SolutionId, Features)  
                                            VALUES (newguid, source.SolutionId, source.Features);";

                await databaseConnection.ExecuteAsync(updateSql, new { solutionId = updateSolutionFeaturesRequest.Id, features = updateSolutionFeaturesRequest.Features });
            }
        }

        /// <summary>
        /// Adds or updates the client application marketing details of a solution.
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

            using (IDbConnection databaseConnection = await DbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
            {
                const string updateSql = @" IF EXISTS (SELECT 1 FROM Solution WHERE Id = @solutionId)
                                        MERGE SolutionDetail AS target  
                                        USING (SELECT @solutionId, @clientApplication) AS source (SolutionId, ClientApplication)
                                        ON (target.Id = source.SolutionDetailId)  
                                        WHEN MATCHED THEN
                                            UPDATE
                                            SET  ClientApplication = source.ClientApplication
                                        WHEN NOT MATCHED THEN
                                            INSERT (SolutionId, ClientApplication)  
                                            VALUES (source.SolutionId, source.ClientApplication);";

                await databaseConnection.ExecuteAsync(updateSql, new { solutionId = updateSolutionClientApplicationRequest.Id, clientApplication = updateSolutionClientApplicationRequest.ClientApplication });
            }
        }
    }
}
