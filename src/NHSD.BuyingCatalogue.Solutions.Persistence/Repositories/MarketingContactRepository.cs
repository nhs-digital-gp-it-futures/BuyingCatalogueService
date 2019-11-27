using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    public sealed class MarketingContactRepository : IMarketingContactRepository
    {
        /// <summary>
        /// Database connection factory to provide new connections.
        /// </summary>
        private readonly IDbConnector _dbConnector;

        public MarketingContactRepository(IDbConnector dbConnector)
        => _dbConnector = dbConnector ?? throw new System.ArgumentNullException(nameof(dbConnector));

        private const string sql = @"SELECT 
                                    MarketingContact.Id
                                    ,MarketingContact.SolutionId
                                    ,MarketingContact.FirstName
                                    ,MarketingContact.LastName
                                    ,MarketingContact.Email
                                    ,MarketingContact.PhoneNumber
                                    ,MarketingContact.Department
                                    FROM Solution
                                    INNER JOIN MarketingContact ON MarketingContact.SolutionId = Solution.Id
                                    WHERE Solution.Id = @solutionId";

        public async Task<IEnumerable<IMarketingContactResult>> BySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
                => await _dbConnector.QueryAsync<MarketingContactResult>(cancellationToken, sql, new { solutionId });
    }
}
