using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;
using NHSD.BuyingCatalogue.SolutionLists.Persistence.Models;

namespace NHSD.BuyingCatalogue.SolutionLists.Persistence.Repositories
{
    /// <summary>
    /// Represents the data access layer for the Solution entity.
    /// </summary>
    public sealed class SolutionListRepository : ISolutionListRepository
    {
        private readonly IDbConnector _dbConnector;

        public SolutionListRepository(IDbConnector dbConnector) => _dbConnector = dbConnector;

        private const string sql = @"SELECT Solution.Id as SolutionId, 
                                        Solution.Name as SolutionName,
                                        SolutionDetail.Summary as SolutionSummary,
                                        Supplier.Id as SupplierId,
                                        Supplier.Name as SupplierName,
                                        Capability.CapabilityRef as CapabilityReference,
                                        Capability.Name as CapabilityName,
                                        Capability.Description as CapabilityDescription,
                                        FrameworkSolutions.IsFoundation as IsFoundation
                                FROM    Solution 
                                        INNER JOIN Supplier ON Supplier.Id = Solution.SupplierId
                                        INNER JOIN SolutionCapability ON Solution.Id = SolutionCapability.SolutionId
                                        INNER JOIN Capability ON Capability.Id = SolutionCapability.CapabilityId
                                        LEFT JOIN SolutionDetail ON Solution.Id = SolutionDetail.SolutionId AND SolutionDetail.Id = Solution.SolutionDetailId
                                        LEFT JOIN FrameworkSolutions ON Solution.Id = FrameworkSolutions.SolutionId
                                WHERE   Solution.PublishedStatusId = 3";

        /// <summary>
        /// Gets a list of <see cref="ISolutionListResult"/> objects.
        /// </summary>
        /// <returns>A list of <see cref="ISolutionListResult"/> objects.</returns>
        public async Task<IEnumerable<ISolutionListResult>> ListAsync(bool foundationOnly, CancellationToken cancellationToken)
            => await _dbConnector.QueryAsync<SolutionListResult>(foundationOnly ? sql + " AND COALESCE(FrameworkSolutions.IsFoundation, 0) = 1" : sql, cancellationToken).ConfigureAwait(false);
    }
}
