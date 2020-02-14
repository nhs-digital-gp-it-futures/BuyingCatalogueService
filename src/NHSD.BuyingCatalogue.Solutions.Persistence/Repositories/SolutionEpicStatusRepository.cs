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
        private readonly IDbConnector _dbConnector;

        public SolutionEpicStatusRepository(IDbConnector dbConnector) =>
            _dbConnector = dbConnector;
        
        private const string CheckStatusExist = @"SELECT COUNT(Id)
                                                  FROM SolutionEpicStatus
                                                  WHERE Name in @statusNames";

        public async Task<int> GetMatchingEpicStatusAsync(IEnumerable<string> statusNames, CancellationToken cancellationToken) =>
            (await _dbConnector.QueryAsync<int>(CheckStatusExist, cancellationToken, new {statusNames})
                .ConfigureAwait(false)).FirstOrDefault();
    }
}
