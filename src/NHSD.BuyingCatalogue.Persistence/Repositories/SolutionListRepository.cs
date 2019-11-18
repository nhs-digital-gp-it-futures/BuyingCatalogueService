using System.Collections.Generic;
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
    public sealed class SolutionListRepository : ISolutionListRepository
    {
        private readonly DbConnector _dbConnector;

        public SolutionListRepository(DbConnector dbConnector) => _dbConnector = dbConnector;

        private const string sql = @"SELECT Solution.Id as SolutionId, 
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

        /// <summary>
        /// Gets a list of <see cref="ISolutionListResult"/> objects.
        /// </summary>
        /// <returns>A list of <see cref="ISolutionListResult"/> objects.</returns>
        public async Task<IEnumerable<ISolutionListResult>> ListAsync(bool foundationOnly, CancellationToken cancellationToken)
            => await _dbConnector.QueryAsync<SolutionListResult>(cancellationToken,
                foundationOnly ? sql + " WHERE COALESCE(FrameworkSolutions.IsFoundation, 0) = 1" : sql);
    }
}
