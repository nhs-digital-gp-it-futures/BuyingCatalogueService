using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Solution
{
    [Binding]
    internal sealed class ListSolutionsSteps
    {
        private const string ListSolutionsUrl = "http://localhost:5200/api/v1/Solutions";

        private readonly Response response;

        public ListSolutionsSteps(Response response)
        {
            this.response = response;
        }

        [When(@"a GET request is made containing no selection criteria")]
        public async Task WhenAGetRequestIsMadeContainingNoSelectionCriteria()
        {
            response.Result = await Client.GetAsync(ListSolutionsUrl);
        }

        [When(@"a GET request is made containing a filter on supplierID (.*)")]
        public async Task WhenAGetRequestIsMadeContainingNoSelectionCriteriaWithFilterOnSupplierId(string supplierId)
        {
            response.Result = await Client.GetAsync($"{ListSolutionsUrl}?supplierId={supplierId}");
        }

        [When(@"a POST request is made containing no selection criteria")]
        public async Task WhenAPostRequestIsMadeContainingNoSelectionCriteria()
        {
            await SendPostRequest(await BuildRequestAsync(new List<string>()));
        }

        [When(@"a POST request is made containing a single capability '(.*)'")]
        public async Task WhenAPostRequestIsMadeContainingASingleCapability(string capability)
        {
            await SendPostRequest(await BuildRequestAsync(new List<string> { capability }));
        }

        [When(@"a POST request is made containing the capabilities (.*)")]
        public async Task WhenARequestContainingTheCapabilities(List<string> capabilities)
        {
            await SendPostRequest(await BuildRequestAsync(capabilities));
        }

        [Then(@"the solutions (.*) are found in the response")]
        public async Task ThenTheSolutionsAreFoundInTheResponse(List<string> solutions)
        {
            solutions = solutions.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            var content = await response.ReadBody();
            content.SelectToken("solutions")?.Select(t => t.SelectToken("name")?.ToString()).Should().BeEquivalentTo(solutions);
        }

        [Then(@"the solutions (.*) are not found in the response")]
        public async Task ThenTheSolutionsAreNotFoundInTheResponse(List<string> solutions)
        {
            var content = await response.ReadBody();
            foreach (var solution in solutions)
            {
                content.SelectToken("solutions")?.Select(t => t.SelectToken("name")?.ToString()).Should().NotContain(solution);
            }
        }

        [Then(@"the details of the solutions returned are as follows")]
        public async Task ThenTheDetailsOfTheSolutionsReturnedAreAsFollows(Table table)
        {
            var expectedSolutions = table.CreateSet<SolutionDetailsTable>().ToList();
            var solutions = (await response.ReadBody()).SelectToken("solutions");

            Assert.NotNull(solutions);
            solutions.Count().Should().Be(expectedSolutions.Count);

            foreach (var expectedSolution in expectedSolutions)
            {
                var solution = solutions.First(t => t.SelectToken("id")?.ToString() == expectedSolution.SolutionId);
                solution.SelectToken("name")?.ToString().Should().Be(expectedSolution.SolutionName);
                solution.SelectToken("summary")?.ToString().Should().Be(expectedSolution.SummaryDescription);
                solution.SelectToken("supplier.name")?.ToString().Should().Be(expectedSolution.SupplierName);
                solution.SelectToken("capabilities")?.Select(t => t.SelectToken("name")?.ToString())
                    .Should().BeEquivalentTo(expectedSolution.Capabilities.Split(",").Select(t => t.Trim()));

                solution.SelectToken("isFoundation")?.ToString().Should().Be(expectedSolution.IsFoundation.ToString(
                    CultureInfo.InvariantCulture));
            }
        }

        [Then(@"an empty solution is returned")]
        public async Task ThenAnEmptySolutionIsReturned()
        {
            var solutions = (await response.ReadBody()).SelectToken("solutions");
            solutions.Should().BeEmpty();
        }

        private static async Task<SolutionsRequest> BuildRequestAsync(IEnumerable<string> capabilityNames)
        {
            var capabilities = await CapabilityEntity.FetchAllAsync();
            var listOfReferences = capabilityNames.Select(cn => capabilities.First(c => c.Name == cn).CapabilityRef);
            return new SolutionsRequest { Capabilities = listOfReferences.Select(r => new CapabilityReference(r)).ToList() };
        }

        private async Task SendPostRequest(SolutionsRequest solutionsRequest)
        {
            response.Result = await Client.PostAsJsonAsync(ListSolutionsUrl, solutionsRequest);
        }

        private sealed class SolutionsRequest
        {
            [UsedImplicitly]
            public List<CapabilityReference> Capabilities { get; init; }
        }

        private sealed class CapabilityReference
        {
            public CapabilityReference(string reference)
            {
                Reference = reference;
            }

            [UsedImplicitly]
            public string Reference { get; }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class SolutionDetailsTable
        {
            public string SolutionId { get; set; }

            public string SolutionName { get; set; }

            public string SummaryDescription { get; set; }

            public string SupplierName { get; set; }

            public string Capabilities { get; set; }

            public bool IsFoundation { get; set; }
        }
    }
}
