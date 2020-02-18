using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    public sealed class SolutionEpicStatusRepository : ISolutionEpicStatusRepository
    {
        private const string CheckStatusExistSql = @"SELECT COUNT(Id)
                                                  FROM SolutionEpicStatus
                                                  WHERE Name in @statusNames";
        private readonly IDbConnector _dbConnector;

        public SolutionEpicStatusRepository(IDbConnector dbConnector) =>
            _dbConnector = dbConnector;

        public async Task<int> CountMatchingEpicStatusAsync(IEnumerable<string> statusNames, CancellationToken cancellationToken) =>
            (await _dbConnector.QueryAsync<int>(CheckStatusExistSql, cancellationToken, new {statusNames})
                .ConfigureAwait(false)).FirstOrDefault();
    }
}
