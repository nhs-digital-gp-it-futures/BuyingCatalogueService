using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Models;

namespace NHSD.BuyingCatalogue.Persistence.Repositories
{
    public sealed class SolutionCapabilityRepository : ISolutionCapabilityRepository
    {
        private readonly DbConnector _dbConnector;

        public SolutionCapabilityRepository(DbConnector dbConnector) => _dbConnector = dbConnector;

        private const string sql = @"SELECT Capability.Id as CapabilityId,
                                        Capability.Name as CapabilityName,
                                        Capability.Description as CapabilityDescription
                                FROM SolutionCapability
                                     INNER JOIN Capability ON SolutionCapability.CapabilityId = Capability.Id
                                WHERE SolutionCapability.SolutionId = @solutionId
                                ORDER BY Capability.Name";

        public async Task<IEnumerable<ISolutionCapabilityListResult>> ListSolutionCapabilities(string solutionId, CancellationToken cancellationToken)
            => await _dbConnector.QueryAsync<SolutionCapabilityListResult>(cancellationToken, sql, new{solutionId});
    }
}
