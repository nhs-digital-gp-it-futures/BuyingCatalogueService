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
        private const string CheckStatusExistSql = @"
            SELECT COUNT(Id)
              FROM dbo.SolutionEpicStatus
             WHERE [Name] in @statusNames;";

        private readonly IDbConnector dbConnector;

        public SolutionEpicStatusRepository(IDbConnector dbConnector) => this.dbConnector = dbConnector;

        public async Task<int> CountMatchingEpicStatusAsync(IEnumerable<string> statusNames, CancellationToken cancellationToken)
        {
            return (await dbConnector.QueryAsync<int>(
                CheckStatusExistSql,
                cancellationToken,
                new { statusNames })).FirstOrDefault();
        }
    }
}
