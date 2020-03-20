using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;

namespace NHSD.BuyingCatalogue.Data.Infrastructure
{
    internal sealed class DbConnector : IDbConnector
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<DbConnector> _logger;


        public DbConnector(IDbConnectionFactory dbConnectionFactory, ILogger<DbConnector> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, CancellationToken cancellationToken, object args = null)
        {
            try
            {
                using var databaseConnection = await _dbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false);
                var result = (await databaseConnection.QueryAsync<T>(sql, args).ConfigureAwait(false)).ToList();

                LogDatabaseWithStatistics(databaseConnection, LoggingEvents.GetItem,
                    "Query {sql} with params: {@args} with ConnectionTime {ConnectionTime}ms, ExecutionTime is {ExecutionTime}",
                    sql, args);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ERROR - Query Async {@sql}", sql);
                throw;
            }
        }

        public async Task ExecuteAsync(string sql, CancellationToken cancellationToken, object args = null)
        {
            try
            {
                using var databaseConnection = await _dbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false);
                await databaseConnection.ExecuteAsync(sql, args).ConfigureAwait(false);

                LogDatabaseWithStatistics(databaseConnection, LoggingEvents.UpdateItem,
                    "Execute {sql} with params: {@args} with ConnectionTime {ConnectionTime}ms, ExecutionTime is {ExecutionTime}",
                    sql, args);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ERROR - Execute Async {@sql}", sql);
                throw;
            }
        }

        public async Task ExecuteMultipleWithTransactionAsync(IEnumerable<(string sql, object args)> functions, CancellationToken cancellationToken)
        {
            try
            {
                using var databaseConnection =
                    await _dbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false);
                var transaction = databaseConnection.BeginTransaction();
                foreach (var (sql, args) in functions)
                {
                    await databaseConnection.ExecuteAsync(sql, args, transaction).ConfigureAwait(false);
                }

                transaction.Commit();
                LogDatabaseWithStatistics(databaseConnection, LoggingEvents.UpdateWithMultipleDocuments,
                    "Execute {sql}  {@functions} with ConnectionTime {ConnectionTime}ms, ExecutionTime is {ExecutionTime}",
                    "Multiple", functions);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ERROR - ExecuteMultipleWithTransactions");
                throw;
            }
        }

        private void LogDatabaseWithStatistics(IDbConnection databaseConnection, int logId, string messageTemplate, string sql ,object args)
        {
            IDictionary stats = null;
            if (databaseConnection is SqlConnection sqlConnection)
            {
                stats = sqlConnection.RetrieveStatistics();
                _logger.LogInformation(logId,
                    messageTemplate,
                    sql, args, stats["ConnectionTime"], stats["ExecutionTime"]);
            }
        }
    }
}
