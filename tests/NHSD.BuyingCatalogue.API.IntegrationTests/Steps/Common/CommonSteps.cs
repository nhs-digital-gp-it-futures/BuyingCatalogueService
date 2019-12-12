using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common
{
    [Binding]
    internal sealed class CommonSteps
    {
        private readonly Response _response;

        [StepArgumentTransformation]
        internal static List<string> TransformToListOfString(string commaSeparatedList) =>
            commaSeparatedList.Split(",").Select(t => t.Trim()).ToList();

        [StepArgumentTransformation]
        internal static DateTime ParseDateTimeString(string dateString) =>
            DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        public CommonSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the string value of element (.*) is (.*)")]
        public async Task ThenTheStringIs(string token, string requirement)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.Value<string>(token).Should().Be(requirement);
        }

        [Then(@"the (.*) string does not exist")]
        public async Task ThenTheBrowserHardwareRequirementsValueIsNull(string token)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.Value<string>(token).Should().Be(null);
        }


        [Then(@"the (required|maxLength) field only contains (.*)")]
        public async Task ThenTheFieldOnlyContains(string token, List<string> listing)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken(token).Select(x => x.ToString()).Should().BeEquivalentTo(listing);
        }

        [Then(@"the (required|maxLength) field contains (.*)")]
        public async Task ThenTheFieldContains(string token, List<string> listing)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken(token).Select(x => x.ToString()).Should().Contain(listing);
        }

        [Then(@"the (required|maxLength) field does not contain (.*)")]
        public async Task ThenTheRequiredFieldDoesNotContain(string token, string field)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken(token).Select(x => x.ToString()).Should().NotContain(field);
        }
    }
}
