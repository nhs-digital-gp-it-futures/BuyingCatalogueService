using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    internal static class SqlRunner
    {
        internal static async Task ExecuteAsync(string connectionString, string sql)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var command = new SqlCommand(sql, sqlConnection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
