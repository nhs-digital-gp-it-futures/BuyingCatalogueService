using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Tools;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Drivers
{
    internal static class BuyingCatalogueService
    {
        private const string WaitServerUrl = "http://localhost:8080/health/dependencies";

        private static readonly string SolutionWorkingDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\..\\"));
        private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(60);

        internal static async Task StartAsync(string serviceConnectionString)
        {
            await DockerComposeProcess.Create(SolutionWorkingDirectory, "-f docker-compose.yml -f docker-compose.integration.yml build"
                , new KeyValuePair<string, string>("NHSD_BUYINGCATALOGUE_DB", serviceConnectionString)
                , new KeyValuePair<string, string>("NHSD_BUYINGCATALOGUE_DB_PASSWORD", DataConstants.SAPassword)).ExecuteAsync((x) => TestContext.WriteLine(x), (x) => TestContext.WriteLine(x));

            TestContext.WriteLine("");
            TestContext.WriteLine("------------------");
            TestContext.WriteLine("");

            await DockerComposeProcess.Create(SolutionWorkingDirectory, "-f docker-compose.yml -f docker-compose.integration.yml up -d"
                , new KeyValuePair<string, string>("NHSD_BUYINGCATALOGUE_DB", serviceConnectionString)
                , new KeyValuePair<string, string>("NHSD_BUYINGCATALOGUE_DB_PASSWORD", DataConstants.SAPassword)).ExecuteAsync((x) => TestContext.WriteLine(x), (x) => TestContext.WriteLine(x));
        }

        internal static async Task WaitAsync()
        {
            var started = await HttpClientAwaiter.WaitForGetAsync(WaitServerUrl, TestTimeout);
            if (!started)
            {
                throw new Exception($"Start Buying Catalogue API failed, could not get a successful health status from '{WaitServerUrl}' after trying for '{TestTimeout}'");
            }
        }

        internal static async Task StopAsync()
        {
            await DockerComposeProcess.Create(SolutionWorkingDirectory, "-f docker-compose.yml -f docker-compose.integration.yml down -v").ExecuteAsync((x) => Debug.WriteLine(x));
        }
    }
}
