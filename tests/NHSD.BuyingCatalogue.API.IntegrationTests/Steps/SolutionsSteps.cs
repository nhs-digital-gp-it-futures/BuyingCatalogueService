using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class SolutionsSteps
    {
        private const string ListSolutionsUrl = "http://localhost:8080/api/v1/Solutions";

        private readonly ScenarioContext _context;

        private readonly Response _response;

        public SolutionsSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [Given(@"Organisations exist")]
        public async Task GivenOrganisationsExist(Table table)
        {
            foreach (var organisation in table.CreateSet<OrganisationTable>())
            {
                await InsertOrganisationAsync(organisation);
            }
        }

        private async Task InsertOrganisationAsync(OrganisationTable organisationTable)
        {
            await OrganisationEntityBuilder.Create()
                .WithName(organisationTable.Name)
                .WithId(organisationTable.Name.Substring(3))
                .Build()
                .InsertAsync();
        }

        [Given(@"Solutions exist")]
        public async Task GivenSolutionsExist(Table table)
        {
            var organisations = await OrganisationEntity.FetchAllAsync();

            foreach (var solutionTable in table.CreateSet<SolutionTable>())
            {
                await SolutionEntityBuilder.Create()
                    .WithName(solutionTable.SolutionName)
                    .WithId(solutionTable.SolutionID)
                    .WithSummary(solutionTable.SummaryDescription)
                    .WithOrganisationId(organisations.First(o => o.Name == solutionTable.OrganisationName).Id)
                    .Build()
                    .InsertAsync();
            }
        }

        [Given(@"Solutions are linked to Capabilities")]
        public async Task GivenSolutionsAreLinkedToCapabilities(Table table)
        {
            var solutions = await SolutionEntity.FetchAllAsync();
            var capabilities = await CapabilityEntity.FetchAllAsync();

            foreach (var solutionCapabilityTable in table.CreateSet<SolutionCapabilityTable>())
            {
                await SolutionCapabilityEntityBuilder.Create()
                    .WithSolutionId(solutions.First(s => s.Name == solutionCapabilityTable.Solution).Id)
                    .WithCapabilityId(capabilities.First(s => s.Name == solutionCapabilityTable.Capability).Id)
                    .Build()
                    .InsertAsync();
            }
        }

        [When(@"a GET request is made containing no selection criteria")]
        public async Task WhenAGETRequestIsMadeContainingNoSelectionCriteria()
        {
            _response.Result = await Client.GetAsync(ListSolutionsUrl);
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

        private async Task<SolutionsRequest> BuildRequestAsync(IEnumerable<string> capabilityNames)
        {
            var capabilities = await CapabilityEntity.FetchAllAsync();
            return new SolutionsRequest { Capabilities = new HashSet<string>(capabilityNames.Select(cn => capabilities.First(c => c.Name == cn).Id.ToString())) };
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
            var expectedSolutions = table.CreateSet<SolutionDetailsTable>();
            var solutions = (await _response.ReadBody()).SelectToken("solutions");

            foreach (var expectedSolution in expectedSolutions)
            {
                var solution = solutions.First(t => t.SelectToken("id").ToString() == expectedSolution.SolutionID);
                solution.SelectToken("name").ToString().Should().Be(expectedSolution.SolutionName);
                solution.SelectToken("summary").ToString().Should().Be(expectedSolution.SummaryDescription);
                solution.SelectToken("organisation.name").ToString().Should().Be(expectedSolution.OrganisationName);
                solution.SelectToken("capabilities").Select(t => t.SelectToken("name").ToString()).Should().BeEquivalentTo(expectedSolution.Capabilities.Split(",").Select(t => t.Trim()));
            }
        }

        [StepArgumentTransformation]
        public List<string> TransformToListOfString(string commaSeparatedList)
        {
            return commaSeparatedList.Split(",").Select(t => t.Trim()).ToList();
        }

        private class SolutionTable
        {
            public string SolutionID { get; set; }

            public string SolutionName { get; set; }

            public string SummaryDescription { get; set; }

            public string OrganisationName { get; set; }
        }

        private class SolutionCapabilityTable
        {
            public string Solution { get; set; }

            public string Capability { get; set; }
        }

        private class OrganisationTable
        {
            public string Name { get; set; }
        }

        private class SolutionsRequest
        {
            public HashSet<string> Capabilities { get; set; }
        }

        private class SolutionDetailsTable
        {
            public string SolutionID { get; set; }

            public string SolutionName { get; set; }

            public string SummaryDescription { get; set; }

            public string OrganisationName { get; set; }

            public string Capabilities { get; set; }
        }
    }
}
