using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data;
using TechTalk.SpecFlow;
using WireMock.Admin.Mappings;


namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.HealthChecks
{
    [Binding]
    internal sealed class HealthChecksSteps
    {
        private readonly Response _response;
        private readonly ScenarioContext _context;
        private const string BaseUrl = "http://localhost:8080";
        private const string ScenarioContextMappingKey = "HealthCheckMappingGuids";

        public HealthChecksSteps(Response response, ScenarioContext context)
        {
            _response = response;
            _context = context;
        }

        [Given(@"The DocumentAPI is (up|down)")]
        public async Task GivenTheDocumentApiIs(string state)
        {
            MappingModel model =
                new MappingModel
                {
                    Response = new ResponseModel {StatusCode = state == "up" ? 200 : 404},
                    Request = new RequestModel
                    {
                        Path = new PathModel
                        {
                            Matchers = new[]
                            {
                                new MatcherModel
                                {
                                    Name = "WildcardMatcher",
                                    Pattern = "/health/live",
                                    IgnoreCase = true
                                }
                            }
                        },
                        Methods = new[] {"GET"}
                    }
                };

            await DocumentApiSetup.SendModel(model, _context, ScenarioContextMappingKey).ConfigureAwait(false);
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

        [AfterScenario]
        public async Task CleanMappings()
        {
            await DocumentApiSetup.ClearMappings(_context, ScenarioContextMappingKey).ConfigureAwait(false);
        }

        [AfterScenario("4821d")]
        public async Task RestoreDbState()
        {
            await Database.AddUserAsync().ConfigureAwait(false);
        }
    }
}
