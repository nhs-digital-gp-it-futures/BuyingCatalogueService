using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests
{
    [Binding]
    public class IntegrationSetupFixture
    {
        [BeforeTestRun]
        public static async Task OneTimeSetUpAsync()
        {
            await IntegrationTestEnvironment.StartAsync();
        }

        [BeforeScenario()]
        public static async Task SetUpAsync()
        {
            await Database.ClearAsync();
        }

        [AfterTestRun]
        public static async Task OneTimeTearDownAsync()
        {
            await IntegrationTestEnvironment.StopAsync();
        }
    }
}
