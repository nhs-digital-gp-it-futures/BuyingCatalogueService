using System;

namespace NHSD.BuyingCatalogue.Testing.Data
{
    public static class Database
    {
        private static string _name;

        internal static string Name => string.Format("[{0}]", _name);

        public static string ConnectionString => string.Format(ConnectionStrings.GPitFutures, _name);

        internal static string ConnectionStringSetup => string.Format(ConnectionStrings.GPitFuturesSetup, _name);

        public static void Create()
        {
            _name = $"gpitfuture-db-dev.dbtests.{DateTime.Now.ToString("yyyyMMdd.HHmmss.fff")}";

            SqlRunner.ExecuteAsync(ConnectionStrings.Master, $"CREATE DATABASE {Name}").Wait();
            SqlRunner.ExecuteAsync(ConnectionStringSetup, Properties.Resources.Permission).Wait();
            CreateObjects();
        }

        public static void CreateObjects()
        {
            SqlRunner.ExecuteAsync(ConnectionStringSetup, Properties.Resources.Create).Wait();
            SqlRunner.ExecuteAsync(ConnectionStringSetup, Properties.Resources.ReferenceData).Wait();
        }

        public static void Clear()
        {
            SqlRunner.ExecuteAsync(ConnectionStringSetup, Properties.Resources.Clear).Wait();
        }

        public static void Drop()
        {
            SqlRunner.ExecuteAsync(ConnectionStringSetup, "DROP USER NHSD").Wait();
            SqlRunner.ExecuteAsync(ConnectionStrings.Master, $"ALTER DATABASE {Name}  SET SINGLE_USER WITH ROLLBACK IMMEDIATE").Wait();
            SqlRunner.ExecuteAsync(ConnectionStrings.Master, $"DROP DATABASE {Name}").Wait();
            SqlRunner.ExecuteAsync(ConnectionStrings.Master, "DROP LOGIN NHSD").Wait();
        }
    }
}
