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
        private const string BaseWireMockUrl = "http://localhost:9090";

        [Given(@"a document named ([\w-]*) exists with solutionId ([\w-]*)")]
        public async Task GivenANamedDocumentForAGivenSolutionIdExists(string documentName, string solutionId)
        {
            //Send specific route map to wiremock for document
            var api = RestClient.For<IWireMockAdminApi>(new Uri(BaseWireMockUrl));

            MappingModel model = new MappingModel
            {
                Guid = new Guid(), Response = new ResponseModel {Body = $"[\"{documentName}\"]"}
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
            await api.PostMappingAsync(model).ConfigureAwait(false);
        }

        //TODO: Think about tear down (at end of scenario, want to clear the config)
    }
}
