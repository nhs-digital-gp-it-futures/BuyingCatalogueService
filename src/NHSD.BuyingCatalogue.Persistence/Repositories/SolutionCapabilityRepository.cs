using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Models;

namespace NHSD.BuyingCatalogue.Persistence.Repositories
{
    public sealed class SolutionCapabilityRepository : ISolutionCapabilityRepository
    {
        private IDbConnectionFactory DbConnectionFactory { get; }

        public SolutionCapabilityRepository(IDbConnectionFactory dbConnectionFactory)
        {
            DbConnectionFactory =
                dbConnectionFactory ?? throw new System.ArgumentNullException(nameof(dbConnectionFactory));
        }

        public async Task<IEnumerable<ISolutionCapabilityListResult>> ListSolutionCapabilities(string solutionId, CancellationToken cancellationToken)
        {
            using (IDbConnection databaseConnection =
                await DbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
            {
                const string sql = @"SELECT Capability.Id as CapabilityId,
                                        Capability.Name as CapabilityName,
                                        Capability.Description as CapabilityDescription
                                FROM SolutionCapability
                                     INNER JOIN Capability ON SolutionCapability.CapabilityId = Capability.Id
                                WHERE SolutionCapability.SolutionId = @solutionId
                                ORDER BY Capability.Name";

                return await databaseConnection.QueryAsync<SolutionCapabilityListResult>(sql, new{solutionId});
            }
        }
    }
}
