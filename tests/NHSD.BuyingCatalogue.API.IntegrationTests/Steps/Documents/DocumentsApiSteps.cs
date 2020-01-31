using System;
using System.Threading.Tasks;
using RestEase;
using TechTalk.SpecFlow;
using WireMock.Admin.Mappings;
using WireMock.Client;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Documents
{
    [Binding]
    internal sealed class DocumentsApiSteps
    {
        [Given(@"a document named ([\w-]*) exists with solutionId ([\w-]*)")]
        public async Task GivenADocumentNamedRoadmapExists(string documentName, string solutionId)
        {
            //Send specific route map to wiremock for document
            var api = RestClient.For<IWireMockAdminApi>("http://localhost:9090");

            //Think about tear down (at end of scenario, want to clear the config)
            var settings = await api.GetSettingsAsync().ConfigureAwait(false);

            var result = await api.GetMappingsAsync().ConfigureAwait(false);

            MappingModel model = new MappingModel
            {
                Guid = new Guid(),
                Response = new ResponseModel
                {
                    Body = "[]"
                }
            };

            model.Request = new RequestModel
            {
                Path = new PathModel
                {
                    Matchers = new[] { new MatcherModel { Name = "WildcardMatcher", Pattern = "/api/v1/solutions/{@solutionId}/documents", IgnoreCase = true } }
                },
                Methods = new[] { "GET" }
            };
            var postResult = await api.PostMappingAsync(model).ConfigureAwait(false);
            var resultMapping = await api.GetMappingsAsync().ConfigureAwait(false);
            Console.WriteLine($"{resultMapping}");
        }

    }
}
