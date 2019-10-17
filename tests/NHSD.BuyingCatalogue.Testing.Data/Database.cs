using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data
{
    public static class Database
    {
        public static async Task CreateAsync()
        {
            await ConnectionAwaiter.AwaitConnectionAsync(ConnectionStrings.Master, 30);
            await SqlRunner.ExecuteAsync(ConnectionStrings.Master, $"CREATE DATABASE [{DataConstants.DatabaseName}]");
            await BuildDatabaseAsync();
        }

        private static async Task BuildDatabaseAsync()
        {
            await ConnectionAwaiter.AwaitConnectionAsync(ConnectionStrings.GPitFuturesSetup, 30);
            await SqlRunner.ExecuteAsync(ConnectionStrings.GPitFuturesSetup, Properties.Resources.User);
            await SqlRunner.ExecuteAsync(ConnectionStrings.GPitFuturesSetup, Properties.Resources.Create);
            await SqlRunner.ExecuteAsync(ConnectionStrings.GPitFuturesSetup, Properties.Resources.ReferenceData);
        }

        public static async Task ClearAsync()
        {
            await SqlRunner.ExecuteAsync(ConnectionStrings.GPitFuturesSetup, Properties.Resources.Clear);
            await SqlRunner.ExecuteAsync(ConnectionStrings.GPitFuturesSetup, "ALTER SERVER ROLE sysadmin ADD MEMBER [NHSD];");
        }

        public static async Task DropServerRoleAsync()
        {
            await SqlRunner.ExecuteAsync(ConnectionStrings.GPitFuturesSetup, "ALTER SERVER ROLE sysadmin DROP MEMBER [NHSD];");
        }

        public static async Task DropAsync()
        {
            await DropUserAsync();
            await DropDatabaseAsync();
        }

        private static async Task DropUserAsync()
        {
            await SqlRunner.ExecuteAsync(ConnectionStrings.GPitFuturesSetup, "DROP USER NHSD");
            await SqlRunner.ExecuteAsync(ConnectionStrings.Master, "DROP LOGIN NHSD");
        }

        private static async Task DropDatabaseAsync()
        {
            await SqlRunner.ExecuteAsync(ConnectionStrings.Master, $"ALTER DATABASE [{DataConstants.DatabaseName}]  SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            await SqlRunner.ExecuteAsync(ConnectionStrings.Master, $"DROP DATABASE [{DataConstants.DatabaseName}]");
        }
    }
}
