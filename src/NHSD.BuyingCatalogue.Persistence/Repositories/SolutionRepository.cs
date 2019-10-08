using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Models;

namespace NHSD.BuyingCatalogue.Persistence.Repositories
{
    /// <summary>
    /// Represents the data access layer for the <see cref="Solution"/> entity.
    /// </summary>
    public sealed class SolutionRepository : ISolutionRepository
    {
        /// <summary>
        /// Database connection factory to provide new connections.
        /// </summary>
        private IDbConnectionFactory DbConnectionFactory { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionRepository"/> class.
        /// </summary>
        public SolutionRepository(IDbConnectionFactory dbConnectionFactory)
        {
            DbConnectionFactory = dbConnectionFactory ?? throw new System.ArgumentNullException(nameof(dbConnectionFactory));
        }

        /// <summary>
        /// Gets a list of <see cref="ISolutionListResult"/> objects.
        /// </summary>
        /// <returns>A list of <see cref="ISolutionListResult"/> objects.</returns>
        public async Task<IEnumerable<ISolutionListResult>> ListAsync(CancellationToken cancellationToken)
        {
            using (IDbConnection databaseConnection = await DbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
            {
                const string sql = @"SELECT Solution.Id as SolutionId, 
                                            Solution.Name as SolutionName,
                                            Solution.Summary as SolutionSummary,
                                            Organisation.Id as OrganisationId,
                                            Organisation.Name as OrganisationName,
                                            Capability.Id as CapabilityId,
                                            Capability.Name as CapabilityName,
                                            Capability.Description as CapabilityDescription
                                    FROM    Solution 
                                            INNER JOIN Organisation ON Organisation.Id = Solution.OrganisationId
                                            INNER JOIN SolutionCapability ON Solution.Id = SolutionCapability.SolutionId
                                            INNER JOIN Capability ON Capability.Id = SolutionCapability.CapabilityId";


                return await databaseConnection.QueryAsync<SolutionListResult>(sql);
            }
        }

        /// <summary>
        /// Gets a <see cref="ISolutionResult"/> matching the specified ID.
        /// </summary>
        /// <param name="id">The ID of a <see cref="ISolutionResult"/>.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to retrieve a <see cref="Solution"/> matching the specified ID.</returns>
        public async Task<ISolutionResult> ByIdAsync(string id, CancellationToken cancellationToken)
        {
            using (IDbConnection databaseConnection = await DbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
            {
                const string sql = @"SELECT Solution.Id,
                                            Solution.Name,
                                            Solution.Summary,
                                            Solution.FullDescription AS Description,
                                            MarketingDetail.AboutUrl AS AboutUrl,
                                            MarketingDetail.Features As Features
                                     FROM   Solution
                                            LEFT OUTER JOIN MarketingDetail ON Solution.Id = MarketingDetail.SolutionId
                                     WHERE  Solution.Id = @id";

                var result = await databaseConnection.QueryAsync<SolutionResult>(sql, new { id });

                return result.SingleOrDefault();
            }
        }

        /// <summary>
        /// Updates the details of the updateSolutionRequest.
        /// </summary>
        /// <param name="updateSolutionRequest">The updated details of a updateSolutionRequest to save to the data store.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to save the specified updateSolutionRequest to the data store.</returns>
        public async Task UpdateAsync(IUpdateSolutionRequest updateSolutionRequest, CancellationToken cancellationToken)
        {
            if (updateSolutionRequest is null)
            {
                throw new System.ArgumentNullException(nameof(updateSolutionRequest));
            }

            using (IDbConnection databaseConnection = await DbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
            {
                using (IDbTransaction transaction = databaseConnection.BeginTransaction())
                {
                    const string updateSolutionSql = @"
                                            UPDATE  Solution
                                            SET     Solution.FullDescription = @description,
                                                    Solution.Summary = @summary
                                            WHERE   Solution.Id = @id;";

                    const string updateMarketingDetailSql = @"
                                         UPDATE  MarketingDetail
                                         SET     MarketingDetail.AboutUrl = @aboutUrl,
                                                 MarketingDetail.Features = @features
                                         WHERE   MarketingDetail.SolutionId = @solutionId;";

                    await databaseConnection.ExecuteAsync(updateSolutionSql, new { id = updateSolutionRequest.Id, description = updateSolutionRequest.Description, summary = updateSolutionRequest.Summary }, transaction);
                    await databaseConnection.ExecuteAsync(updateMarketingDetailSql, new { solutionId = updateSolutionRequest.Id, aboutUrl = updateSolutionRequest.AboutUrl, features = updateSolutionRequest.Features }, transaction);

                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Updates the supplier status of the specified updateSolutionRequest in the data store.
        /// </summary>
        /// <param name="updateSolutionRequest">The updateSolutionRequest to update.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to update the supplier status of the specified updateSolutionRequest in the data store.</returns>
        public async Task UpdateSupplierStatusAsync(IUpdateSolutionSupplierStatusRequest updateSolutionSupplierStatusRequest, CancellationToken cancellationToken)
        {
            if (updateSolutionSupplierStatusRequest is null)
            {
                throw new ArgumentNullException(nameof(updateSolutionSupplierStatusRequest));
            }

            using (IDbConnection databaseConnection = await DbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
            {
                using (IDbTransaction transaction = databaseConnection.BeginTransaction())
                {
                    const string updateSolutionSupplierStatusSql = @"
                                            UPDATE  Solution
                                            SET		Solution.SupplierStatusId = @supplierStatusId
                                            WHERE   Solution.Id = @id;";

                    await databaseConnection.ExecuteAsync(updateSolutionSupplierStatusSql, new { id = updateSolutionSupplierStatusRequest.Id, supplierStatusId = updateSolutionSupplierStatusRequest.SupplierStatusId }, transaction);

                    transaction.Commit();
                }
            }
        }
    }
}
