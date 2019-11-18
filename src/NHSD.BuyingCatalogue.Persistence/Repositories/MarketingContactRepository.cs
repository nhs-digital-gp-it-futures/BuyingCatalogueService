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
    public sealed class MarketingContactRepository : IMarketingContactRepository
    {
        /// <summary>
        /// Database connection factory to provide new connections.
        /// </summary>
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public MarketingContactRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory ?? throw new System.ArgumentNullException(nameof(dbConnectionFactory));
        }

        public async Task<IEnumerable<IMarketingContactResult>> BySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
        {
            using (IDbConnection databaseConnection = await _dbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
            {
                const string sql = @"SELECT 
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

                var result = await databaseConnection.QueryAsync<MarketingContactResult>(sql, new { solutionId });

                return result;
            }
        }
    }
}
