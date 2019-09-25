using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data
{
    public static class ConnectionAwaiter
    {
        public static async Task AwaitConnectionAsync(int timeout = 30)
        {
            var now = DateTime.Now;

            while (DateTime.Now.Subtract(now).TotalSeconds < timeout)
            {
                try
                {
                    using (var sqlConnection = new SqlConnection(ConnectionStrings.Master))
                    {
                        sqlConnection.Open();
                        return;
                    }
                }
                catch
                {
                    await Task.Delay(1000);
                }
            }

            throw new Exception($"Could not connect to database instance after trying for {timeout} seconds");
        }
    }
}
