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
        private readonly Response _response;
        private const string BaseUrl = "http://localhost:8080";

        public HealthChecksSteps(Response response)
        {
            _response = response;
        }

        [Given(@"The Database server is available")]
        public async Task GivenTheDatabaseIsUp()
        {
            await Database.AwaitDatabaseAsync().ConfigureAwait(false);
        }

        [Given(@"The Database server is not available")]
        public async Task GivenTheDatabaseIsDown()
        {
            await Database.DropUserAsync().ConfigureAwait(false);
        }

        [When(@"the dependency health-check endpoint is hit")]
        public async Task WhenTheHealthCheckEndpointIsHit()
        {
            _response.Result = await Client.GetAsync($"{BaseUrl}/health/ready").ConfigureAwait(false);
        }

        [Then(@"the response will be (Degraded|Unhealthy|Healthy)")]
        public async Task ThenTheHealthStatusIs(string status)
        {
            var healthStatus = await _response.Result.Content.ReadAsStringAsync().ConfigureAwait(false);
            healthStatus.Should().Be(status);
        }

        [AfterScenario("4821d")]
        public async Task RestoreDbState()
        {
            await Database.AddUserAsync().ConfigureAwait(false);
        }
    }
}
