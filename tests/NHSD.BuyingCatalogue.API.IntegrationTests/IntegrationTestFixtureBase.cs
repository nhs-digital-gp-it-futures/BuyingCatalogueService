using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Data;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests
{
    public abstract class IntegrationTestFixtureBase
    {
        [SetUp]
        public async Task SetUpAsync()
        {
            await Database.ClearAsync();
        }
    }

    [Binding]
    public class IntegrationTestsSetup
    {
        [BeforeTestRun]
        public static async Task OneTimeSetUpAsync()
        {
            await BuyingCatalogueService.StartAsync(ConnectionStrings.ServiceConnectionString("db"));

            await Database.CreateAsync();

            await BuyingCatalogueService.WaitAsync();
        }

        [BeforeScenario()]
        public static async Task SetUpAsync()
        {
            await Database.ClearAsync();
        }

        [AfterTestRun]
        public static async Task OneTimeTearDownAsync()
        {
            await Database.DropAsync();
            await BuyingCatalogueService.StopAsync();
        }
    }

}
