using System.Linq;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

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
            var defaultStringValueRetriever = Service.Instance.ValueRetrievers.FirstOrDefault(vr => vr is TechTalk.SpecFlow.Assist.ValueRetrievers.StringValueRetriever);
            if (defaultStringValueRetriever != null)
            {
                Service.Instance.ValueRetrievers.Unregister(defaultStringValueRetriever);
            }

            Service.Instance.ValueRetrievers.Register(new StringValueRetriever());
            await Database.ClearAsync();
        }
    }
}
