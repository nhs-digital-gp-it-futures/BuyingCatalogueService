using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class FeaturesSectionValidationSteps
    {
        private const string FeaturesUrl = "http://localhost:8080/api/v1/Solutions/{0}/sections/features";

        private List<string> _features = new List<string>();

        private readonly ScenarioContext _context;

        private readonly Response _response;

        public FeaturesSectionValidationSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [Given(@"a request with ten features")]
        public void GivenARequestWithTenFeatures()
        {
            for (int i = 0; i < 10; i++)
            {
                _features.Add("");
            }
        }

        [Given(@"feature at position (.*) is a string of (.*) characters")]
        public void GivenFeatureAtPositionIsAStringOfCharacters(int position, int length)
        {
            // List is zero based, so need to minus one.
            _features[position - 1] = GenerateStringOfLength(length);
        }

        [When(@"the update features request is made for (.*)")]
        public async Task WhenTheUpdateFeaturesRequestIsMadeForSln(string featuresId)
        {
            var content = new
            {
                listing = _features
            };

            _response.Result = await Client.PutAsJsonAsync(string.Format(FeaturesUrl, featuresId), content).ConfigureAwait(false);
        }

        [Then(@"the features response required field contains (.*)")]
        public async Task ThenTheFeaturesResponseRequiredFieldContainsListing(List<string> listing)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("maxLength").Select(x => x.ToString()).Should().BeEquivalentTo(listing);
        }

        private static string GenerateStringOfLength(int length)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                builder.Append("a");
            }

            return builder.ToString();
        }
    }
}
