using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class SubmitForReviewSteps
    {
        private const string SubmitForReviewSolutionsUrl = "http://localhost:8080/api/v1/Solutions/{0}/SubmitForReview";

        private readonly ScenarioContext _context;

        private readonly Response _response;

        public SubmitForReviewSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [When(@"a request is made to submit Solution (.*) for review")]
        public async Task WhenARequestIsMadeToSubmitSlnForReview(string solutionId)
        {
            _response.Result = await Client.PutAsync(string.Format(SubmitForReviewSolutionsUrl, solutionId));
        }

        [When(@"a request is made to submit Solution for review with no solution id")]
        public async Task WhenARequestIsMadeToSubmitForReviewWithNoSolutionId()
        {
            _response.Result = await Client.PutAsync(string.Format(SubmitForReviewSolutionsUrl, " "));
        }
    }
}
