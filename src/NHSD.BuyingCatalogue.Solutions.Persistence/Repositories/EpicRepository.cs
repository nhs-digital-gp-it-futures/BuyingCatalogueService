using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    public sealed class EpicRepository : IEpicRepository
    {
        private readonly IDbConnector _dbConnector;

        public EpicRepository(IDbConnector dbConnector) =>
            _dbConnector = dbConnector;

        private const string CheckIdsExist = @"SELECT COUNT(Id)
                                               FROM Epic
                                               WHERE Id in @epicIds";

        public async Task<int> GetMatchingEpicIdsAsync(IEnumerable<string> epicIds, CancellationToken cancellationToken) =>
            (await _dbConnector.QueryAsync<int>(CheckIdsExist, cancellationToken, new { epicIds })
                .ConfigureAwait(false)).FirstOrDefault();
    }
}
