using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Drivers;
using NHSD.BuyingCatalogue.Testing.Data;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.IntegrationTests
{
    public abstract class IntegrationTestFixtureBase
    {
        [OneTimeSetUp]
        public async Task OneTimeSetUpAsync()
        {
            await Database.InitialiseAsync("db");
            await BuyingCatalogueService.StartAsync();

            await Database.CreateAsync();

            await BuyingCatalogueService.WaitAsync();
        }

        [SetUp]
        public async Task SetUpAsync()
        {
            await Database.ClearAsync();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDownAsync()
        {
            await Database.DropAsync();
            await BuyingCatalogueService.StopAsync();
        }
    }
}
