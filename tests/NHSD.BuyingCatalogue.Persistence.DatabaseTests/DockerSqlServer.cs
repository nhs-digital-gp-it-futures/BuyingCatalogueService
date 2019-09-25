using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Tools;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    internal class DockerSqlServer
    {
        public static async Task Start(string serverName = "localhost")
        {
            DockerComposeProcess.Create(AppDomain.CurrentDomain.BaseDirectory, "-f docker-compose.yml up -d", new KeyValuePair<string, string>("NHSD_BUYINGCATALOGUE_DB_PASSWORD", Database.SAPassword)).ExecuteAsync();

            await ConnectionAwaiter.AwaitConnectionAsync(serverName, 30);
        }

        internal static void Stop()
        {
            DockerComposeProcess.Create(AppDomain.CurrentDomain.BaseDirectory, "-f docker-compose.yml down -v").ExecuteAsync();
        }
    }
}

