using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Support
{
    public static class IntegrationTestEnvironment
    {
        public static async Task StartAsync()
        {
            await Task.WhenAll(BuyingCatalogueService.AwaitApiRunningAsync(),
                               DocumentService.AwaitApiRunningAsync())
                .ConfigureAwait(false);
        }
    }
}
