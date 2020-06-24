using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace NHSD.BuyingCatalogue.Testing.Data
{
    internal static class SqlRunner
    {
        internal static async Task ExecuteAsync(string connectionString, string sql)
        {
            var sqlCommands = sql.Split("GO");

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                foreach (var command in sqlCommands)
                {
                    await sqlConnection.ExecuteAsync(command).ConfigureAwait(false);
                }
            }
        }

        internal static async Task ExecuteAsync<T>(string connectionString, string sql, T instance)
        {
            using (IDbConnection databaseConnection = new SqlConnection(connectionString))
            {
                await databaseConnection.ExecuteAsync(sql, instance).ConfigureAwait(false);
            }
        }

        internal static async Task<IEnumerable<T>> FetchAllAsync<T>(string selectSql, object param = null)
        {
            using (IDbConnection databaseConnection = new SqlConnection(ConnectionStrings.GPitFuturesSetup))
            {
                return (await databaseConnection.QueryAsync<T>(selectSql, param).ConfigureAwait(false)).ToList();
            }
        }

        internal static async Task<T> QueryFirstAsync<T>(string sql, object parameters = null)
        {
            using IDbConnection databaseConnection = new SqlConnection(ConnectionStrings.GPitFuturesSetup);
            return await databaseConnection.QueryFirstAsync<T>(sql, parameters);
        }
    }
}
