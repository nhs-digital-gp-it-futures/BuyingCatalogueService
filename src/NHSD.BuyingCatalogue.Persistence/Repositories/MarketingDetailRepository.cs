using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;

namespace NHSD.BuyingCatalogue.Persistence.Repositories
{
    /// <summary>
    /// Represents the data access layer for the <see cref="Solution"/> entity.
    /// </summary>
    public sealed class MarketingDetailRepository : IMarketingDetailRepository
    {
        /// <summary>
        /// Database connection factory to provide new connections.
        /// </summary>
        private IDbConnectionFactory DbConnectionFactory { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionRepository"/> class.
        /// </summary>
        public MarketingDetailRepository(IDbConnectionFactory dbConnectionFactory)
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
                                        MERGE MarketingDetail AS target  
                                        USING (SELECT @solutionId, @features) AS source (SolutionId, Features)
                                        ON (target.SolutionId = source.SolutionId)  
                                        WHEN MATCHED THEN
                                            UPDATE
                                            SET  Features = source.Features
                                        WHEN NOT MATCHED THEN
                                            INSERT (SolutionId, Features)  
                                            VALUES (source.SolutionId, source.Features);";

                await databaseConnection.ExecuteAsync(updateSql, new { solutionId = updateSolutionFeaturesRequest.Id, features = updateSolutionFeaturesRequest.Features });
            }
        }

    }
}
