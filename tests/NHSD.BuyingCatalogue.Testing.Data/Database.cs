using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data
{
    public static class Database
    {
        public static async Task AwaitDatabaseAsync()
        {
            await AwaitConnection.AwaitAsync(ConnectionStrings.ServiceConnectionString());
        }

        public static async Task ClearAsync()
        {
            await SqlRunner.ExecuteAsync(ConnectionStrings.GPitFuturesSetup, "EXEC test.ClearData;");
        }

        public static async Task DropServerRoleAsync()
        {
            await SqlRunner.ExecuteAsync(ConnectionStrings.GPitFuturesSetup, "EXEC test.DropRole;");
        }

        public static async Task DropUserAsync()
        {
            await SqlRunner.ExecuteAsync(ConnectionStrings.GPitFuturesSetup, "EXEC test.DropUserAndLogin;");
        }

        public static async Task AddUserAsync()
        {
            await SqlRunner.ExecuteAsync(ConnectionStrings.GPitFuturesSetup, "EXEC test.RestoreUserAndLogin;");
        }
    }
}
