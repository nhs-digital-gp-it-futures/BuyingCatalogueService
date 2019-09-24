using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Data;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    [SetUpFixture]
    internal sealed class DatabaseTestsSetup
    {
        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            await DockerSqlServer.Start();
            await Database.Create();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await Database.Drop();
            DockerSqlServer.Stop();
        }
    }
}
