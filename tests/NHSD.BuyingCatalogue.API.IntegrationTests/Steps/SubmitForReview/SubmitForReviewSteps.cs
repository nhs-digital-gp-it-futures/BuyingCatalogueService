using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.SubmitForReview
{
    [Binding]
    internal sealed class SubmitForReviewSteps
    {
        private const string SubmitForReviewSolutionsUrl = "http://localhost:5200/api/v1/Solutions/{0}/SubmitForReview";

        private readonly Response response;

        public SubmitForReviewSteps(Response response)
        {
            this.response = response;
        }

        [When(@"a request is made to submit Solution (.*) for review")]
        public async Task WhenARequestIsMadeToSubmitSlnForReview(string solutionId)
        {
            response.Result = await Client.PutAsync(
                string.Format(CultureInfo.InvariantCulture, SubmitForReviewSolutionsUrl, solutionId));
        }

        [When(@"a request is made to submit Solution for review with no solution id")]
        public async Task WhenARequestIsMadeToSubmitForReviewWithNoSolutionId()
        {
            response.Result = await Client.PutAsync(
                string.Format(CultureInfo.InvariantCulture, SubmitForReviewSolutionsUrl, " "));
        }

        [Then(@"the response details of the submit Solution for review request are as follows")]
        public async Task ThenTheDetailsOfTheSolutionsReturnedAreAsFollows(Table table)
        {
            var body = await response.ReadBody();

            var expectedSectionErrorTable = table.CreateSet<SubmitSolutionForReviewResponseTable>();
            foreach (var expectedSectionError in expectedSectionErrorTable)
            {
                var token = body.SelectToken(expectedSectionError.Property);

                Assert.NotNull(token);
                token.Select(s => s.ToString()).Should().BeEquivalentTo(expectedSectionError.InvalidSections);
            }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class SubmitSolutionForReviewResponseTable
        {
            public string Property { get; init; }

            public List<string> InvalidSections { get; init; }
        }
    }
}
