using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    public sealed class SolutionFrameworkRepository : ISolutionFrameworkRepository
    {
        private const string GetFrameworkBySolutionIdSql = @"
            SELECT f.Id,
                   f.ShortName as FrameworkName
            FROM dbo.FrameworkSolutions AS fs
                 INNER JOIN dbo.Framework AS f 
                    ON fs.FrameworkId=f.Id
            WHERE fs.SolutionId = @solutionId;";

        private readonly IDbConnector dbConnector;

        public SolutionFrameworkRepository(IDbConnector dbConnector) => this.dbConnector = dbConnector;

        public async Task<IEnumerable<ISolutionFrameworkListResult>> GetFrameworkBySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
        {
            return (await dbConnector.QueryAsync<SolutionFrameworkListResult>(
                GetFrameworkBySolutionIdSql,
                cancellationToken,
                new { solutionId })).ToList();
        }
    }
}
