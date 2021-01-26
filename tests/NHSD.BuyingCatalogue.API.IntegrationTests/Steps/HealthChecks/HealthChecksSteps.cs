using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.HealthChecks
{
    [Binding]
    internal sealed class HealthChecksSteps
    {
        private const string BaseUrl = "http://localhost:5200";

        private readonly Response response;
        private readonly ScenarioContext context;

        public HealthChecksSteps(Response response, ScenarioContext context)
        {
            this.response = response;
            this.context = context;
        }

        [Given(@"The Database server is available")]
        public static async Task GivenTheDatabaseIsUp()
        {
            await Database.AwaitDatabaseAsync();
        }

        [Given(@"The Database server is not available")]
        public async Task GivenTheDatabaseIsDown()
        {
            await Database.DropUserAsync();
            context["IsUserDropped"] = true;
        }

        [When(@"the dependency health-check endpoint is hit")]
        public async Task WhenTheHealthCheckEndpointIsHit()
        {
            response.Result = await Client.GetAsync($"{BaseUrl}/health/ready");
        }

        [Then(@"the response will be (Degraded|Unhealthy|Healthy)")]
        public async Task ThenTheHealthStatusIs(string status)
        {
            var healthStatus = await response.Result.Content.ReadAsStringAsync();
            healthStatus.Should().Be(status);
        }

        [AfterScenario]
        public async Task RestoreDbState()
        {
            context.TryGetValue("IsUserDropped", out bool isUserDropped);
            if (isUserDropped)
            {
                await Database.AddUserAsync();
                context["IsUserDropped"] = false;
            }
        }
    }
}
