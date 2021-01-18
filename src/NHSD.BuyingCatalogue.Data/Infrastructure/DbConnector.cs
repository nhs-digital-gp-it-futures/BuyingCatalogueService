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
        private readonly IDbConnectionFactory dbConnectionFactory;
        private readonly ILogger<DbConnector> logger;

        public DbConnector(IDbConnectionFactory dbConnectionFactory, ILogger<DbConnector> logger)
        {
            this.dbConnectionFactory = dbConnectionFactory;
            this.logger = logger;
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, CancellationToken cancellationToken, object param = null)
        {
            try
            {
                using var databaseConnection = await dbConnectionFactory.GetAsync(cancellationToken);
                var result = await databaseConnection.QueryFirstOrDefaultAsync<T>(sql, param);

                LogDatabaseWithStatistics(
                    databaseConnection,
                    LoggingEvents.GetItem,
                    "Query {sql} with params: {@param} with ConnectionTime {ConnectionTime}ms, ExecutionTime is {ExecutionTime}",
                    sql,
                    param);

                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e, "ERROR - Query First Async or Default {@sql}", sql);
                throw;
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, CancellationToken cancellationToken, object args = null)
        {
            try
            {
                using var databaseConnection = await dbConnectionFactory.GetAsync(cancellationToken);
                var result = (await databaseConnection.QueryAsync<T>(sql, args)).ToList();

                LogDatabaseWithStatistics(
                    databaseConnection,
                    LoggingEvents.GetItem,
                    "Query {sql} with params: {@args} with ConnectionTime {ConnectionTime}ms, ExecutionTime is {ExecutionTime}",
                    sql,
                    args);

                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e, "ERROR - Query Async {@sql}", sql);
                throw;
            }
        }

        public async Task ExecuteAsync(string sql, CancellationToken cancellationToken, object args = null)
        {
            try
            {
                using var databaseConnection = await dbConnectionFactory.GetAsync(cancellationToken);
                await databaseConnection.ExecuteAsync(sql, args);

                LogDatabaseWithStatistics(
                    databaseConnection,
                    LoggingEvents.UpdateItem,
                    "Execute {sql} with params: {@args} with ConnectionTime {ConnectionTime}ms, ExecutionTime is {ExecutionTime}",
                    sql,
                    args);
            }
            catch (Exception e)
            {
                logger.LogError(e, "ERROR - Execute Async {@sql}", sql);
                throw;
            }
        }

        public async Task ExecuteMultipleWithTransactionAsync(IEnumerable<(string Sql, object Args)> functions, CancellationToken cancellationToken)
        {
            try
            {
                using var databaseConnection = await dbConnectionFactory.GetAsync(cancellationToken);
                var transaction = databaseConnection.BeginTransaction();
                foreach ((string sql, object args) in functions)
                {
                    await databaseConnection.ExecuteAsync(sql, args, transaction);
                }

                transaction.Commit();
                LogDatabaseWithStatistics(
                    databaseConnection,
                    LoggingEvents.UpdateWithMultipleDocuments,
                    "Execute {sql}  {@functions} with ConnectionTime {ConnectionTime}ms, ExecutionTime is {ExecutionTime}",
                    "Multiple",
                    functions);
            }
            catch (Exception e)
            {
                logger.LogError(e, "ERROR - ExecuteMultipleWithTransactions");
                throw;
            }
        }

        private void LogDatabaseWithStatistics(IDbConnection databaseConnection, int logId, string messageTemplate, string sql, object args)
        {
            if (databaseConnection is not SqlConnection sqlConnection)
                return;

            IDictionary stats = sqlConnection.RetrieveStatistics();
            logger.LogInformation(
                logId,
                messageTemplate,
                sql,
                args,
                stats["ConnectionTime"],
                stats["ExecutionTime"]);
        }
    }
}
