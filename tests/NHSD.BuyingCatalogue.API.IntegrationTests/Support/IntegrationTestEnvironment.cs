using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Drivers;
using NHSD.BuyingCatalogue.Testing.Data;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Support
{
    public static class IntegrationTestEnvironment
    {
        public static async Task StartAsync()
        {
            //await BuyingCatalogueService.StartAsync();
            await BuyingCatalogueService.AwaitApiRunningAsync();
        }

        public static async Task StopAsync()
        {
            await BuyingCatalogueService.StopAsync();
            await BuyingCatalogueService.CleanPublishDirectory();
        }
    }
}
