using System;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    internal static class Database
    {
        private static string _name;

        internal static string Name => string.Format("[{0}]", _name);

        internal static string ConnectionString => string.Format(ConnectionStrings.GPitFutures, _name);

        internal static void Create()
        {
            _name = $"gpitfuture-db-dev.dbtests.{DateTime.Now.ToString("yyyyMMdd.HHmmss.fff")}";

            SqlRunner.ExecuteAsync(ConnectionStrings.Master, $"CREATE DATABASE {Name}").Wait();
            CreateObjects();
        }

        internal static void CreateObjects()
        {
            SqlRunner.ExecuteAsync(ConnectionString, Properties.Resources.Create).Wait();
            SqlRunner.ExecuteAsync(ConnectionString, Properties.Resources.ReferenceData).Wait();
        }

        internal static void Clear()
        {
            SqlRunner.ExecuteAsync(ConnectionString, Properties.Resources.Clear).Wait();
        }

        internal static void Drop()
        {
            SqlRunner.ExecuteAsync(ConnectionStrings.Master, $"ALTER DATABASE {Name}  SET SINGLE_USER WITH ROLLBACK IMMEDIATE").Wait();
            SqlRunner.ExecuteAsync(ConnectionStrings.Master, $"DROP DATABASE {Name}").Wait();
        }
    }
}
