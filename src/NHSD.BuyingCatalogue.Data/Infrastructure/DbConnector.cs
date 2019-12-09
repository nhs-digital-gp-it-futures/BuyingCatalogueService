using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace NHSD.BuyingCatalogue.Data.Infrastructure
{
    internal sealed class DbConnector : IDbConnector
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DbConnector(IDbConnectionFactory dbConnectionFactory) => _dbConnectionFactory = dbConnectionFactory;

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, CancellationToken cancellationToken, object args = null)
        {
            using var databaseConnection = await _dbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false);
            return (await databaseConnection.QueryAsync<T>(sql, args).ConfigureAwait(false)).ToList();
        }

        public async Task ExecuteAsync(string sql, CancellationToken cancellationToken, object args = null)
        {
            using var databaseConnection = await _dbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false);
            await databaseConnection.ExecuteAsync(sql, args).ConfigureAwait(false);
        }
        public async Task ExecuteMultipleWithTransactionAsync(IEnumerable<(string sql, object args)> functions, CancellationToken cancellationToken)
        {
            using var databaseConnection = await _dbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false);
            var transaction = databaseConnection.BeginTransaction();
            foreach (var (sql, args) in functions)
            {
                await databaseConnection.ExecuteAsync(sql, args, transaction).ConfigureAwait(false);
            }
            transaction.Commit();
        }
    }
}
