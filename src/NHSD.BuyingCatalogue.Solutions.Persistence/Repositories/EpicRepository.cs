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
        private const string CheckIdsExistSql = @"SELECT COUNT(Id)
                                               FROM Epic
                                               WHERE Id in @epicIds";

        private readonly IDbConnector _dbConnector;

        public EpicRepository(IDbConnector dbConnector) =>
            _dbConnector = dbConnector;

        public async Task<int> CountMatchingEpicIdsAsync(IEnumerable<string> epicIds,
            CancellationToken cancellationToken) =>
            (await _dbConnector.QueryAsync<int>(CheckIdsExistSql, cancellationToken, new {epicIds})
                .ConfigureAwait(false)).FirstOrDefault();
    }
}
