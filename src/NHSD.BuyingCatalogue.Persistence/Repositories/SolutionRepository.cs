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
    /// Represents the data access layer for the Solution entity.
    /// </summary>
    public sealed class SolutionRepository : ISolutionRepository
    {
        private readonly DbConnector _dbConnector;

        public SolutionRepository(DbConnector dbConnector) => _dbConnector = dbConnector;

        /// <summary>
        /// Gets a <see cref="ISolutionResult"/> matching the specified ID.
        /// </summary>
        /// <param name="id">The ID of a <see cref="ISolutionResult"/>.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to retrieve a <see cref="ISolutionResult"/> matching the specified ID.</returns>
        public async Task<ISolutionResult> ByIdAsync(string id, CancellationToken cancellationToken)
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

            var result = await _dbConnector.QueryAsync<SolutionResult>(cancellationToken, sql, new { id });

            return result.SingleOrDefault();
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

            const string updateSolutionSupplierStatusSql = @"
                                        UPDATE  Solution
                                        SET		Solution.SupplierStatusId = @supplierStatusId
                                        WHERE   Solution.Id = @id
                                IF @@ROWCOUNT = 0
                                    THROW 60000, 'Solution or SolutionDetail not found', 1; ";

            await _dbConnector.ExecuteAsync(cancellationToken, updateSolutionSupplierStatusSql, updateSolutionSupplierStatusRequest);
        }
    }
}
