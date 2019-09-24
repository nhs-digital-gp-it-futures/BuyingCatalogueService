using System;
using System.Collections.Generic;
using System.IO;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Tools;
using Shouldly;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Drivers
{
    public class BuyingCatalogueService
    {
        private const string WaitServerUrl = "http://localhost:8080/health/dependencies";

        private static readonly KeyValuePair<string, string> TagEnvironmentVariable = new KeyValuePair<string, string>("Tag", "test");
        private static readonly string SolutionWorkingDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\..\\"));
        private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(60);

        private readonly DockerComposeProcess _dockerComposeProcess;

        public BuyingCatalogueService()
        {
            _dockerComposeProcess = new DockerComposeProcess(SolutionWorkingDirectory, new List<string> { "docker-compose.yml", "docker-compose.integration.yml" }
            , new Dictionary<string, string>
            {
                { TagEnvironmentVariable.Key, TagEnvironmentVariable.Value },
                { "NHSD_BUYINGCATALOGUE_DB", Database.ConnectionString },
            });
        }

        public void Start()
        {
            _dockerComposeProcess.Start().ShouldBe(0);
        }

        public void Wait()
        {
            var started = HttpClientAwaiter.WaitForGetAsync(WaitServerUrl, TestTimeout).Result;
            if (!started)
            {
                throw new Exception($"Start Buying Catalogue API failed, could not get a successful health status from '{WaitServerUrl}' after trying for '{TestTimeout}'");
            }
        }

        internal void Stop()
        {
            _dockerComposeProcess.Stop().ShouldBe(0);
        }
    }
}
