using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Support
{
    public static class IntegrationTestEnvironment
    {
        public static async Task StartAsync()
        {
            await BuyingCatalogueService.AwaitApiRunningAsync().ConfigureAwait(false);
            //TODO: Check Wiremock / DOc API is running

        }
    }
}
