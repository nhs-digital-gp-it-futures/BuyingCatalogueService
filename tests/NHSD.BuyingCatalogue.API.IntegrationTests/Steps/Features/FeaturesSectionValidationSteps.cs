using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Features
{
    [Binding]
    internal sealed class FeaturesSectionValidationSteps
    {
        private const string FeaturesUrl = "http://localhost:5200/api/v1/Solutions/{0}/sections/features";

        private readonly List<string> features = new();
        private readonly Response response;

        public FeaturesSectionValidationSteps(Response response)
        {
            this.response = response;
        }

        [Given(@"a request with ten features")]
        public void GivenARequestWithTenFeatures()
        {
            for (int i = 0; i < 10; i++)
            {
                features.Add($"{i}");
            }
        }

        [Given(@"feature at position (.*) is a string of (.*) characters")]
        public void GivenFeatureAtPositionIsAStringOfCharacters(int position, int length)
        {
            // List is zero based, so need to minus one.
            features[position - 1] = GenerateStringOfLength(length);
        }

        [When(@"the update features request is made for (.*)")]
        public async Task WhenTheUpdateFeaturesRequestIsMadeForSln(string featuresId)
        {
            var content = new
            {
                listing = features,
            };

            response.Result = await Client.PutAsJsonAsync(
                string.Format(CultureInfo.InvariantCulture, FeaturesUrl, featuresId),
                content);
        }

        private static string GenerateStringOfLength(int length)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                builder.Append('a');
            }

            return builder.ToString();
        }
    }
}
