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

        public DocumentsApiSteps(ScenarioContext context)
        {
            _context = context;
        }

        private const string BaseWireMockUrl = "http://localhost:9090";

        [Given(@"a document named ([\w-]*) exists with solutionId ([\w-]*)")]
        public async Task GivenANamedDocumentForAGivenSolutionIdExists(string documentName,
            string solutionId)
        {
            MappingModel model = new MappingModel {Response = new ResponseModel {Body = $"[\"{documentName}\"]"} };
            
            model.Request = new RequestModel
            {
                Path = new PathModel
                {
                    Matchers = new[]
                    {
                        new MatcherModel
                        {
                            Name = "WildcardMatcher",
                            Pattern = $"/api/v1/solutions/{solutionId}/documents",
                            IgnoreCase = true
                        }
                    }
                },
                Methods = new[] {"GET"}
            };
            await SendModel(model, _context).ConfigureAwait(false);
        }

        [Given(@"the document api fails with solutionId ([\w-]*)")]
        public async Task GivenTheDocumentApiFailsWithSolutionId(string solutionId)
        {
            MappingModel model = new MappingModel
            {
                Response = new ResponseModel {StatusCode = 500, Body = "Demo Error"}
            };

            model.Request = new RequestModel
            {
                Path = new PathModel
                {
                    Matchers = new[]
                    {
                        new MatcherModel
                        {
                            Name = "WildcardMatcher",
                            Pattern = $"/api/v1/solutions/{solutionId}/documents",
                            IgnoreCase = true
                        }
                    }
                },
                Methods = new[] {"GET"}
            };

            await SendModel(model, _context).ConfigureAwait(false);
        }

        private static async Task SendModel(MappingModel model, ScenarioContext context)
        {
            model.Guid = Guid.NewGuid();
            model.Priority = 10;
            var api = RestClient.For<IWireMockAdminApi>(new Uri(BaseWireMockUrl));
            var result = await api.PostMappingAsync(model).ConfigureAwait(false);
            result.Status.Should().Be("Mapping added");
            if (!context.ContainsKey("DocumentApiMappingGuids"))
                context["DocumentApiMappingGuids"] = new List<Guid>();

            if (context["DocumentApiMappingGuids"] is List<Guid> guidList)
                guidList.Add(model.Guid.Value);
        }

        [AfterScenario()]
        public async Task ClearMappings()
        {
            if (_context.ContainsKey("DocumentApiMappingGuids") && _context["DocumentApiMappingGuids"] is List<Guid> guidList)
            {
                var api = RestClient.For<IWireMockAdminApi>(new Uri(BaseWireMockUrl));
                await Task.WhenAll(guidList.Select(g => api.DeleteMappingAsync(g)).ToArray()).ConfigureAwait(false);
            }
        }
    }
}
