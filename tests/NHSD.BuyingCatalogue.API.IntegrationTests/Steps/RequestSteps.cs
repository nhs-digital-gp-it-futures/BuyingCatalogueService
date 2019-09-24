using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    public sealed class RequestSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _context;

        public RequestSteps(ScenarioContext context)
        {
            _context = context;
        }

        [When(@"the request is made")]
        public void WhenTheRequestIsMade()
        {
            _context.Pending();
        }

        [Then(@"a successful response is returned")]
        public void ThenASuccessfulResponseIsReturned()
        {
            _context.Pending();
        }
    }
}
