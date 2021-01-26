using System;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Tools;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Support
{
    internal static class BuyingCatalogueService
    {
        private const string WaitServerUrl = "http://localhost:5200/health/live";

        private const string WaitServerUrlDependencies = "http://localhost:5200/health/ready";

        private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(60);

        internal static async Task AwaitApiRunningAsync()
        {
            await AwaitApiRunningAsync(WaitServerUrl);
            await AwaitApiRunningAsync(WaitServerUrlDependencies);
        }

        internal static async Task AwaitApiRunningAsync(string url)
        {
            var started = await AwaitHttpClient.WaitForGetAsync(url, TestTimeout);
            if (!started)
            {
                throw new TimeoutException(
                    $"Start Buying Catalogue API failed, could not get a successful health status from '{url}' after trying for '{TestTimeout}'");
            }
        }
    }
}
