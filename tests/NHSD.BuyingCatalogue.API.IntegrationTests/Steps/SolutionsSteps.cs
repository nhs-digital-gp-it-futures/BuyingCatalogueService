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
        private const string SubmitForReviewSolutionsUrl = "http://localhost:8080/api/v1/Solutions/{0}/SubmitForReview";

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
                    .WithFullDescription(solutionTable.FullDescription)
                    .WithOrganisationId(organisations.First(o => o.Name == solutionTable.OrganisationName).Id)
                    .WithSupplierStatusId(solutionTable.SupplierStatusId)
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

        [Given(@"a Solution (.*) does not exist")]
        public async Task GivenASolutionSlnDoesNotExist(string solutionId)
        {
            var solutionList = await SolutionEntity.FetchAllAsync();
            solutionList.Select(x => x.Id).Should().NotContain(solutionId);
        }

        [Then(@"Solutions exist")]
        public async Task ThenSolutionsExist(Table table)
        {
            var expectedSolutions = table.CreateSet<SolutionUpdatedTable>();
            var solutions = await SolutionEntity.FetchAllAsync();
            solutions.Select(s => new
            {
                SolutionID = s.Id,
                SolutionName = s.Name,
                SummaryDescription = s.Summary,
                s.FullDescription
            }).Should().BeEquivalentTo(expectedSolutions);
        }

        [Then(@"the field \[SupplierStatusId] for Solution (.*) should correspond to '(.*)'")]
        public async Task ThenFieldSolutionSupplierStatusIdShouldCorrespondTo(string solutionId, string supplierStatusName)
        {
            var status = await SolutionSupplierStatusEntity.GetByNameAsync(supplierStatusName);
            var solution = await SolutionEntity.GetByIdAsync(solutionId);

            solution.SupplierStatusId.Should().Be(status.Id);
        }

        [Then(@"the solution contains SummaryDescription of '(.*)'")]
        public async Task ThenTheSolutionContainsSummaryDescriptionOf(string summary)
        {
            var content = await _response.ReadBody();
            content.SelectToken("solution.marketingData.sections[?(@.id == 'solution-description')].data.summary").ToString().Should().Be(summary);
        }

        [Then(@"the solution contains FullDescription of '(.*)'")]
        public async Task ThenTheSolutionContainsFullDescriptionOf(string description)
        {
            var content = await _response.ReadBody();
            content.SelectToken("solution.marketingData.sections[?(@.id == 'solution-description')].data.description").ToString().Should().Be(description);
        }

        [Then(@"the solution (features|solution-description) section status is (COMPLETE|INCOMPLETE)")]
        public async Task ThenTheSolutionFeaturesSectionStatusIsCOMPLETE(string section, string status)
        {
            var content = await _response.ReadBody();
            content.SelectToken($"solution.marketingData.sections[?(@.id == '{section}')].status").ToString().Should().Be(status);
        }

        [Then(@"the solution (features|solution-description) section requirement is (Mandatory|Optional)")]
        public async Task ThenTheSolutionSectionRequirementIsMandatory(string section, string requirement)
        {
            var content = await _response.ReadBody();
            content.SelectToken($"solution.marketingData.sections[?(@.id == '{section}')].requirement").ToString().Should().Be(requirement);
        }

        [Then(@"the solution (features|solution-description) section Mandatory field list is")]
        public async Task ThenTheSolutionSolution_DescriptionSectionMandatoryFieldListIs(string section, Table table)
        {
            var content = await _response.ReadBody();
            content.SelectToken($"solution.marketingData.sections[?(@.id == '{section}')].mandatory")
                .Select(s => s.ToString()).Should().BeEquivalentTo(table.CreateSet<MandatoryTable>().Select(s => s.Mandatory));
        }

        [Then(@"the solution (features|solution-description) section Mandatory field list is empty")]
        public async Task ThenTheSolutionFeaturesSectionMandatoryFieldListIsEmpty(string section)
        {
            var content = await _response.ReadBody();
            content.SelectToken($"solution.marketingData.sections[?(@.id == '{section}')].mandatory")
                .Select(s => s.ToString()).Should().BeNullOrEmpty();
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

            public string FullDescription { get; set; }

            public int SupplierStatusId { get; set; }
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

        private class SolutionUpdatedTable
        {
            public string SolutionID { get; set; }

            public string SolutionName { get; set; }

            public string SummaryDescription { get; set; }

            public string FullDescription { get; set; }
        }

        private class MandatoryTable
        {
            public string Mandatory { get; set; }
        }
    }
}
