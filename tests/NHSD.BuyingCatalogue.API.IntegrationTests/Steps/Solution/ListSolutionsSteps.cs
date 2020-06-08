using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Solution
{
    [Binding]
    internal sealed class ListSolutionsSteps
    {
        private const string ListSolutionsUrl = "http://localhost:5200/api/v1/Solutions";

        private readonly Response _response;

        public ListSolutionsSteps(Response response)
        {
            _response = response;
        }

        [When(@"a GET request is made containing no selection criteria")]
        public async Task WhenAGETRequestIsMadeContainingNoSelectionCriteria()
        {
            _response.Result = await Client.GetAsync(ListSolutionsUrl);
        }

        [When(@"a GET request is made containing a filter on supplierID (.*)")]
        public async Task WhenAGETRequestIsMadeContainingNoSelectionCriteriaWithFilterOnSupplierId(string supplierID)
        {
            _response.Result = await Client.GetAsync($"{ListSolutionsUrl}?supplierId={supplierID}");
        }

        [When(@"a POST request is made containing no selection criteria")]
        public async Task WhenAPOSTRequestIsMadeContainingNoSelectionCriteria()
        {
            await SendPostRequest(await BuildRequestAsync(new List<string>()));
        }

        [When(@"a POST request is made containing a single capability '(.*)'")]
        public async Task WhenAPOSTRequestIsMadeContainingASingleCapability(string capability)
        {
            await SendPostRequest(await BuildRequestAsync(new List<string> { capability }));
        }

        [When(@"a POST request is made containing the capabilities (.*)")]
        public async Task WhenARequestContainingTheCapabilities(List<string> capabilities)
        {
            await SendPostRequest(await BuildRequestAsync(capabilities));
        }

        private async Task SendPostRequest(SolutionsRequest solutionsRequest)
        {
            _response.Result = await Client.PostAsJsonAsync(ListSolutionsUrl, solutionsRequest);
        }

        private static async Task<SolutionsRequest> BuildRequestAsync(IEnumerable<string> capabilityNames)
        {
            var capabilities = await CapabilityEntity.FetchAllAsync();
            var listOfReferences = capabilityNames.Select(cn => capabilities.First(c => c.Name == cn).CapabilityRef);
            return new SolutionsRequest { Capabilities = listOfReferences.Select(r => new CapabilityReference(r)).ToList()};
        }

        [Then(@"the solutions (.*) are found in the response")]
        public async Task ThenTheSolutionsAreFoundInTheResponse(List<string> solutions)
        {
            solutions = solutions.Where(s => !String.IsNullOrWhiteSpace(s)).ToList();
            var content = await _response.ReadBody();
            content.SelectToken("solutions").Select(t => t.SelectToken("name").ToString()).Should().BeEquivalentTo(solutions);
        }

        [Then(@"the solutions (.*) are not found in the response")]
        public async Task ThenTheSolutionsAreNotFoundInTheResponse(List<string> solutions)
        {
            var content = await _response.ReadBody();
            foreach (var solution in solutions)
            {
                content.SelectToken("solutions").Select(t => t.SelectToken("name").ToString()).Should().NotContain(solution);
            }
        }

        [Then(@"the details of the solutions returned are as follows")]
        public async Task ThenTheDetailsOfTheSolutionsReturnedAreAsFollows(Table table)
        {
            var expectedSolutions = table.CreateSet<SolutionDetailsTable>().ToList();
            var solutions = (await _response.ReadBody()).SelectToken("solutions");

            solutions.Count().Should().Be(expectedSolutions.Count());

            foreach (var expectedSolution in expectedSolutions)
            {
                var solution = solutions.First(t => t.SelectToken("id").ToString() == expectedSolution.SolutionId);
                solution.SelectToken("name").ToString().Should().Be(expectedSolution.SolutionName);
                solution.SelectToken("summary")?.ToString().Should().Be(expectedSolution.SummaryDescription);
                solution.SelectToken("supplier.name").ToString().Should().Be(expectedSolution.SupplierName);
                solution.SelectToken("capabilities").Select(t => t.SelectToken("name").ToString()).Should().BeEquivalentTo(expectedSolution.Capabilities.Split(",").Select(t => t.Trim()));
                solution.SelectToken("isFoundation").ToString().Should().Be(expectedSolution.IsFoundation.ToString(CultureInfo.InvariantCulture));
            }
        }

        [Then(@"an empty solution is returned")]
        public async Task ThenAnEmptySolutionIsReturned()
        {
            var solutions = (await _response.ReadBody()).SelectToken("solutions");
            solutions.Should().BeEmpty();
        }

        private class SolutionsRequest
        {
            public List<CapabilityReference> Capabilities { get; set; }
        }

        private class CapabilityReference
        {
            public string Reference { get; }

            public CapabilityReference(string reference)
            {
                Reference = reference;
            }
        }

        private class SolutionDetailsTable
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
