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
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                using (var command = new SqlCommand(sql, sqlConnection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        internal static async Task<IEnumerable<T>> FetchAllAsync<T>(string selectSql)
        {
            using (IDbConnection databaseConnection = new SqlConnection(ConnectionStrings.GPitFuturesSetup))
            {
                return (await databaseConnection.QueryAsync<T>(selectSql).ConfigureAwait(false)).ToList();
            }
        }
    }
}
