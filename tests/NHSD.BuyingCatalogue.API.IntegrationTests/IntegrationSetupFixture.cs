using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.IntegrationTests
{
    [SetUpFixture]
    internal sealed class IntegrationSetupFixture
    {
        [OneTimeSetUp]
        public async Task OneTimeSetUpAsync()
        {
            await IntegrationTestEnvironment.StartAsync();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDownAsync()
        {
            await IntegrationTestEnvironment.StopAsync();
        }
    }
}
