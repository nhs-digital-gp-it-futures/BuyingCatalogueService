using NHSD.BuyingCatalogue.API.IntegrationTests.Drivers;
using NHSD.BuyingCatalogue.Testing.Data;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.IntegrationTests
{
    public abstract class IntegrationTestFixtureBase
    {
        private readonly BuyingCatalogueService _service = new BuyingCatalogueService();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _service.Start();

            Database.Create();

            _service.Wait();
        }

        [SetUp]
        public void SetUp()
        {
            Database.Clear();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Database.Drop();

            _service.Stop();
        }
    }
}
