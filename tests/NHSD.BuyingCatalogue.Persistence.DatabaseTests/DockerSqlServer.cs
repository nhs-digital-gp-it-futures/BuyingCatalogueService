using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Tools;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    internal class DockerSqlServer
    {
        public static async Task StartAsync()
        {
            await DockerComposeProcess.Create(AppDomain.CurrentDomain.BaseDirectory, "-f docker-compose.yml up -d", new KeyValuePair<string, string>("NHSD_BUYINGCATALOGUE_DB_PASSWORD", Database.SAPassword)).ExecuteAsync();
        }

        internal static async Task StopAsync()
        {
            await DockerComposeProcess.Create(AppDomain.CurrentDomain.BaseDirectory, "-f docker-compose.yml down -v").ExecuteAsync();
        }
    }
}

