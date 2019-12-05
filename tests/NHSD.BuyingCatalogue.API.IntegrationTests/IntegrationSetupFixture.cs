using System.Linq;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using DateTimeValueRetriever = NHSD.BuyingCatalogue.API.IntegrationTests.Support.DateTimeValueRetriever;
using StringValueRetriever = NHSD.BuyingCatalogue.API.IntegrationTests.Support.StringValueRetriever;

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

            var defaultDateTimeValueRetriever = Service.Instance.ValueRetrievers.FirstOrDefault(vr => vr is TechTalk.SpecFlow.Assist.ValueRetrievers.DateTimeValueRetriever);
            if (defaultDateTimeValueRetriever != null)
            {
                Service.Instance.ValueRetrievers.Unregister(defaultDateTimeValueRetriever);
            }

            Service.Instance.ValueRetrievers.Register(new DateTimeValueRetriever());
            Service.Instance.ValueRetrievers.Register(new StringValueRetriever());
            await Database.ClearAsync();
        }
    }
}
