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
    /// Represents the data access layer for the Solution entity.
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
        public async Task<IEnumerable<ISolutionListResult>> ListAsync(bool foundationOnly, CancellationToken cancellationToken)
        {
            using (IDbConnection databaseConnection = await DbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
            {
                string sql = @"SELECT Solution.Id as SolutionId, 
                                            Solution.Name as SolutionName,
                                            SolutionDetail.Summary as SolutionSummary,
                                            Organisation.Id as OrganisationId,
                                            Organisation.Name as OrganisationName,
                                            Capability.Id as CapabilityId,
                                            Capability.Name as CapabilityName,
                                            Capability.Description as CapabilityDescription,
                                            FrameworkSolutions.IsFoundation as IsFoundation
                                    FROM    Solution 
                                            INNER JOIN Organisation ON Organisation.Id = Solution.OrganisationId
                                            INNER JOIN SolutionCapability ON Solution.Id = SolutionCapability.SolutionId
                                            INNER JOIN Capability ON Capability.Id = SolutionCapability.CapabilityId
                                            LEFT JOIN SolutionDetail ON Solution.Id = SolutionDetail.SolutionId AND SolutionDetail.Id = Solution.SolutionDetailId
                                            LEFT JOIN FrameworkSolutions ON Solution.Id = FrameworkSolutions.SolutionId";
                if (foundationOnly)
                {
                    sql += " WHERE COALESCE(FrameworkSolutions.IsFoundation, 0) = 1";
                }

                return await databaseConnection.QueryAsync<SolutionListResult>(sql);
            }
        }

        /// <summary>
        /// Gets a <see cref="ISolutionResult"/> matching the specified ID.
        /// </summary>
        /// <param name="id">The ID of a <see cref="ISolutionResult"/>.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to retrieve a <see cref="ISolutionResult"/> matching the specified ID.</returns>
        public async Task<ISolutionResult> ByIdAsync(string id, CancellationToken cancellationToken)
        {
            using (IDbConnection databaseConnection = await DbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
            {
                const string sql = @"SELECT Solution.Id,
                                            Solution.Name,                                           
                                            Organisation.Name as OrganisationName,
                                            SolutionDetail.Summary AS Summary,
                                            SolutionDetail.FullDescription AS Description,
                                            SolutionDetail.AboutUrl AS AboutUrl,
                                            SolutionDetail.Features As Features,
                                            SolutionDetail.ClientApplication as ClientApplication,
                                            FrameworkSolutions.IsFoundation as IsFoundation
                                     FROM   Solution
                                            INNER JOIN Organisation ON Organisation.Id = Solution.OrganisationId
                                            LEFT JOIN SolutionDetail ON Solution.Id = SolutionDetail.SolutionId AND SolutionDetail.Id = Solution.SolutionDetailId
                                            LEFT JOIN FrameworkSolutions ON Solution.Id = FrameworkSolutions.SolutionId
                                     WHERE  Solution.Id = @id";

                var result = await databaseConnection.QueryAsync<SolutionResult>(sql, new { id });

                return result.SingleOrDefault();
            }
        }

        /// <summary>
        /// Updates the supplier status of the specified updateSolutionRequest in the data store.
        /// </summary>
        /// <param name="updateSolutionSupplierStatusRequest">The update solution supplier status details.</param>
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
                const string updateSolutionSupplierStatusSql = @"
                                            UPDATE  Solution
                                            SET		Solution.SupplierStatusId = @supplierStatusId
                                            WHERE   Solution.Id = @id
                                    IF @@ROWCOUNT = 0
                                        THROW 60000, 'Solution or SolutionDetail not found', 1; ";

                await databaseConnection.ExecuteAsync(updateSolutionSupplierStatusSql, updateSolutionSupplierStatusRequest);
            }
        }
    }
}
