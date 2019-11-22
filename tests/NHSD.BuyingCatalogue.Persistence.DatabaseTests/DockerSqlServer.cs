using System;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Tools;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    internal class DockerSqlServer
    {
        public static async Task StartAsync()
        {
            await DockerComposeProcess.Create(AppDomain.CurrentDomain.BaseDirectory, "-f docker-compose.yml up -d").ExecuteAsync();
        }

        internal static async Task StopAsync()
        {
            await DockerComposeProcess.Create(AppDomain.CurrentDomain.BaseDirectory, "-f docker-compose.yml down -v").ExecuteAsync();
        }
    }
}

