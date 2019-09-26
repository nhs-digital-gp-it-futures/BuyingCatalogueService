using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Data;
using NUnit.Framework;

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
}
