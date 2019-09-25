using System;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data
{
    public static class Database
    {
        private static string _serverName;

        private static string _name = "gpitfuture-db-dev";

        internal static string Name => string.Format("[{0}]", _name);

        internal static string ConnectionStringMaster => string.Format(ConnectionStrings.Master, _serverName);

        internal static string ConnectionStringSetup => string.Format(ConnectionStrings.GPitFuturesSetup, _serverName, _name);

        public static string ConnectionString => string.Format(ConnectionStrings.GPitFutures, "db", _name);

        public static string SAPassword => ConnectionStrings.SAPassword;

        public static async Task Create(string serverName = "localhost")
        {
            _serverName = serverName;
            //_name = $"gpitfuture-db-dev.dbtests.{DateTime.Now.ToString("yyyyMMdd.HHmmss.fff")}";

            await SqlRunner.ExecuteAsync(ConnectionStringMaster, $"CREATE DATABASE {Name}");
            await Task.Delay(2000);
            await SqlRunner.ExecuteAsync(ConnectionStringSetup, Properties.Resources.Permission);
            await CreateObjects();
        }

        public static async Task CreateObjects()
        {
            await SqlRunner.ExecuteAsync(ConnectionStringSetup, Properties.Resources.Create);
            await SqlRunner.ExecuteAsync(ConnectionStringSetup, Properties.Resources.ReferenceData);
        }

        public static async Task Clear()
        {
            await SqlRunner.ExecuteAsync(ConnectionStringSetup, Properties.Resources.Clear);
        }

        public static async Task Drop()
        {
            await SqlRunner.ExecuteAsync(ConnectionStringSetup, "DROP USER NHSD");
            await SqlRunner.ExecuteAsync(ConnectionStringMaster, $"ALTER DATABASE {Name}  SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            await SqlRunner.ExecuteAsync(ConnectionStringMaster, $"DROP DATABASE {Name}");
            await SqlRunner.ExecuteAsync(ConnectionStringMaster, "DROP LOGIN NHSD");
        }
    }
}
