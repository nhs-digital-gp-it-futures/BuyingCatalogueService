using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Support
{
    internal static class IntegrationTestEnvironment
    {
        internal static async Task StartAsync()
        {
            await Task.WhenAll(BuyingCatalogueService.AwaitApiRunningAsync(), DocumentService.AwaitApiRunningAsync());
        }
    }
}
