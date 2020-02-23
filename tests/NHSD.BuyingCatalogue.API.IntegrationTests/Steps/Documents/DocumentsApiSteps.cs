using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using TechTalk.SpecFlow;
using WireMock.Admin.Mappings;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Documents
{
    [Binding]
    internal sealed class DocumentsApiSteps
    {
        private readonly ScenarioContext _context;
        private const string ScenarioContextMappingKey = "DocumentApiMappingGuids";

        public DocumentsApiSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Given(@"a document named ([\w-]*) exists with solutionId ([\w-]*)")]
        public async Task GivenANamedDocumentForAGivenSolutionIdExists(string documentName,
            string solutionId)
        {
            MappingModel model = new MappingModel { Response = new ResponseModel { Body = $"[\"{documentName}\"]" } };

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
                Methods = new[] { "GET" }
            };
            await DocumentApiSetup.SendModel(model, _context, ScenarioContextMappingKey).ConfigureAwait(false);
        }

        [Given(@"the document api fails with solutionId ([\w-]*)")]
        public async Task GivenTheDocumentApiFailsWithSolutionId(string solutionId)
        {
            MappingModel model = new MappingModel
            {
                Response = new ResponseModel { StatusCode = 500, Body = "Demo Error" }
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
                Methods = new[] { "GET" }
            };
            await DocumentApiSetup.SendModel(model, _context, ScenarioContextMappingKey).ConfigureAwait(false);
        }

        [AfterScenario]
        public async Task ClearMappings()
        {
            await DocumentApiSetup.ClearMappings(_context, ScenarioContextMappingKey).ConfigureAwait(false);
        }
    }
}
