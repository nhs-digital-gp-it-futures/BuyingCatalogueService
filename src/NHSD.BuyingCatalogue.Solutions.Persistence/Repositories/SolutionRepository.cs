using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    /// <summary>
    /// Represents the data access layer for the Solution entity.
    /// </summary>
    public sealed class SolutionRepository : ISolutionRepository
    {
        private readonly IDbConnector _dbConnector;

        public SolutionRepository(IDbConnector dbConnector) => _dbConnector = dbConnector;

        private const string ByIdSql = @"SELECT s.Id,
       ci.[Name],
       s.LastUpdated,
       ci.PublishedStatusId AS PublishedStatus,
       s.Summary,
       s.FullDescription AS [Description],
       s.AboutUrl,
       s.Features,
       s.RoadMap,
       s.IntegrationsUrl,
       s.ImplementationDetail AS ImplementationTimescales,
       s.ClientApplication,
       s.Hosting,
       s.LastUpdated AS SolutionDetailLastUpdated,
       f.IsFoundation
   FROM dbo.Solution AS s
        INNER JOIN dbo.CatalogueItem AS ci
                ON s.Id = ci.CatalogueItemId
        LEFT OUTER JOIN dbo.FrameworkSolutions AS f
                ON s.Id = f.SolutionId
  WHERE s.Id = @id;";

        private const string DoesSolutionExist = @"SELECT COUNT(*)
  FROM dbo.Solution
 WHERE Id = @id;";

        /// <summary>
        /// Gets a <see cref="ISolutionResult"/> matching the specified ID.
        /// </summary>
        /// <param name="id">The ID of a <see cref="ISolutionResult"/>.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to retrieve a <see cref="ISolutionResult"/> matching the specified ID.</returns>
        public async Task<ISolutionResult> ByIdAsync(string id, CancellationToken cancellationToken)
            => (await _dbConnector.QueryAsync<SolutionResult>(ByIdSql, cancellationToken, new { id })).SingleOrDefault();

        /// <summary>
        /// Updates the supplier status of the specified updateSolutionRequest in the data store.
        /// </summary>
        /// <param name="updateSolutionSupplierStatusRequest">The update solution supplier status details.</param>
        /// <param name="cancellationToken">A token to notify if the task operation should be cancelled.</param>
        /// <returns>A task representing an operation to update the supplier status of the specified updateSolutionRequest in the data store.</returns>
        public Task UpdateSupplierStatusAsync(IUpdateSolutionSupplierStatusRequest updateSolutionSupplierStatusRequest, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Checks if the solution exists
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>True if it exists</returns>
        public async Task<bool> CheckExists(string id, CancellationToken cancellationToken)
        {
            var solutionCount = await _dbConnector.QueryAsync<int>(
                DoesSolutionExist,
                cancellationToken,
                new
                {
                    id,
                });

            return solutionCount.Sum() == 1;
        }
    }
}
