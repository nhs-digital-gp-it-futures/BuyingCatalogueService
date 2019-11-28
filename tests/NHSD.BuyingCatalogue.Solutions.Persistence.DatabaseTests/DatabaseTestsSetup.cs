using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Data;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests
{
    [SetUpFixture]
    internal sealed class DatabaseTestsSetup
    {
        [OneTimeSetUp]
        public async Task OneTimeSetUpAsync()
        {
            await Database.AwaitDatabaseAsync();
        }
    }
}
