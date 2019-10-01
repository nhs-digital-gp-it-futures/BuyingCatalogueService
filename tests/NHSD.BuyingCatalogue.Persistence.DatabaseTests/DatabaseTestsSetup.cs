using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Data;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    [SetUpFixture]
    internal sealed class DatabaseTestsSetup
    {
        [OneTimeSetUp]
        public async Task OneTimeSetUpAsync()
        {
            await DockerSqlServer.StartAsync();
            await Database.CreateAsync();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDownAsync()
        {
            await DockerSqlServer.StopAsync();
        }
    }
}
