using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace NHSD.BuyingCatalogue.Persistence.Infrastructure
{
    public sealed class DbConnector
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DbConnector(IDbConnectionFactory dbConnectionFactory) => _dbConnectionFactory = dbConnectionFactory;

        public async Task<IEnumerable<T>> QueryAsync<T>(CancellationToken cancellationToken, string sql, object args = null)
        {
            using (var databaseConnection = await _dbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
            {
                return (await databaseConnection.QueryAsync<T>(sql, args).ConfigureAwait(false)).ToList();
            }
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken, string sql, object args = null)
        {
            using (var databaseConnection = await _dbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
            {
                await databaseConnection.ExecuteAsync(sql, args);
            }
        }
    }
}
