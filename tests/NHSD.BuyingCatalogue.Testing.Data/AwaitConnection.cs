using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data
{
    internal static class AwaitConnection
    {
        public static async Task AwaitAsync(string connectionString, int timeout = 30)
        {
            var now = DateTime.Now;

            while (DateTime.Now.Subtract(now).TotalSeconds < timeout)
            {
                try
                {
                    await using var sqlConnection = new SqlConnection(connectionString);
                    await sqlConnection.OpenAsync();
                    return;
                }
                catch (SqlException)
                {
                    await Task.Delay(1000);
                }
            }

            throw new TimeoutException($"Could not connect to database instance after trying for {timeout} seconds");
        }
    }
}
