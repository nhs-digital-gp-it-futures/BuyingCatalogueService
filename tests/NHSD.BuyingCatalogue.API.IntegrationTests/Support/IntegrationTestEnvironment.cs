using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Drivers;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Support
{
    public static class IntegrationTestEnvironment
    {
        public static async Task StartAsync()
        {
            await BuyingCatalogueService.AwaitApiRunningAsync().ConfigureAwait(false);
        }
    }
}
