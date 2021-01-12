using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using RestEase;
using TechTalk.SpecFlow;
using WireMock.Admin.Mappings;
using WireMock.Client;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Documents
{
    [Binding]
    internal sealed class DocumentsApiSteps
    {
        private readonly ScenarioContext _context;
        private const string ScenarioContextMappingKey = "DocumentApiMappingGuids";
        private const string WireMockBaseUrl = "http://localhost:5201";

        public DocumentsApiSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Given(@"a document named ([\w-]*) exists with solutionId ([\w-]*)")]
        public async Task GivenANamedDocumentForAGivenSolutionIdExists(string documentName,
            string solutionId)
        {
            var model = CreateMappingModel($"/api/v1/solutions/{solutionId}/documents", 200, $"[\"{documentName}\"]");

            await SendModel(model, _context, ScenarioContextMappingKey).ConfigureAwait(false);
        }

        [Given(@"the document api fails with solutionId ([\w-]*)")]
        public async Task GivenTheDocumentApiFailsWithSolutionId(string solutionId)
        {
            var model = CreateMappingModel($"/api/v1/solutions/{solutionId}/documents", 500, "Demo Error");

            await SendModel(model, _context, ScenarioContextMappingKey).ConfigureAwait(false);
        }

        [Given(@"The document api is (up|down)")]
        public async Task GivenTheDocumentApiIs(string state)
        {
            var model = CreateMappingModel("/health/live", state == "up" ? 200 : 404);

            await SendModel(model, _context, ScenarioContextMappingKey).ConfigureAwait(false);
        }

        private static MappingModel CreateMappingModel(string path, int responseStatusCode, string responseBody = null)
        {
            return new()
            {
                Response = new ResponseModel { StatusCode = responseStatusCode, Body = responseBody },
                Request = new RequestModel
                {
                    Path = new PathModel
                    {
                        Matchers = new[]
                            {
                                new MatcherModel
                                {
                                    Name = "WildcardMatcher",
                                    Pattern = path,
                                    IgnoreCase = true,
                                },
                            },
                    },
                    Methods = new[] { "GET" },
                },
            };
        }

        private static async Task SendModel(MappingModel model, ScenarioContext context, string mappingKey)
        {
            await AddMapping(model).ConfigureAwait(false);

            if (!context.ContainsKey(mappingKey))
                context[mappingKey] = new List<Guid>();

            if (context[mappingKey] is List<Guid> guidList)
                if (model.Guid != null)
                {
                    guidList.Add(model.Guid.Value);
                }
        }

        private static async Task AddMapping(MappingModel model)
        {
            model.Guid = Guid.NewGuid();
            model.Priority = 10;
            var api = RestClient.For<IWireMockAdminApi>(new Uri(WireMockBaseUrl));
            var result = await api.PostMappingAsync(model).ConfigureAwait(false);
            result.Status.Should().Be("Mapping added");
        }

        [AfterScenario]
        public async Task CleanMappings()
        {
            if (_context.ContainsKey(ScenarioContextMappingKey) && _context[ScenarioContextMappingKey] is List<Guid> guidList)
            {
                var api = RestClient.For<IWireMockAdminApi>(new Uri(WireMockBaseUrl));
                await Task.WhenAll(guidList.Select(g => api.DeleteMappingAsync(g)).ToArray()).ConfigureAwait(false);
            }
        }
    }
}
