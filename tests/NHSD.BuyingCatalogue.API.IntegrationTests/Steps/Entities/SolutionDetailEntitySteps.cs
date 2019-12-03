using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    public sealed class SolutionDetailEntitySteps
    {

        [Given(@"SolutionDetail exist")]
        public async Task GivenSolutionDetailExist(Table table)
        {
            foreach (var solutionDetail in table.CreateSet<SolutionDetailTable>())
            {
                await SolutionDetailEntityBuilder.Create()
                    .WithFeatures(solutionDetail.Features)
                    .WithSummary(solutionDetail.SummaryDescription)
                    .WithFullDescription(solutionDetail.FullDescription)
                    .WithAboutUrl(solutionDetail.AboutUrl)
                    .WithSolutionId(solutionDetail.Solution)
                    .WithClientApplication(solutionDetail.ClientApplication)
                    .Build()
                    .InsertAndSetCurrentForSolutionAsync();
            }
        }

        [Given(@"a SolutionDetail (.*) does not exist")]
        public async Task GivenASolutionDetailDoesNotExist(string solutionId)
        {
            var solutionDetailList = await SolutionDetailEntity.FetchAllAsync();
            solutionDetailList.Select(x => x.SolutionId).Should().NotContain(solutionId);
        }

        [Then(@"SolutionDetail exist")]
        public async Task ThenSolutionDetailExist(Table table)
        {
            var expectedSolutionDetails = table.CreateSet<SolutionDetailTable>().Select(m => new
            {
                m.Solution,
                AboutUrl = string.IsNullOrWhiteSpace(m.AboutUrl) ? null : m.AboutUrl,
                Features = string.IsNullOrWhiteSpace(m.Features) ? null : m.Features,
                Summary = string.IsNullOrWhiteSpace(m.SummaryDescription) ? null : m.SummaryDescription,
                FullDescription = string.IsNullOrWhiteSpace(m.FullDescription) ? null : m.FullDescription,
                ClientApplication = string.IsNullOrWhiteSpace(m.ClientApplication) ? null : JToken.Parse(m.ClientApplication).ToString()
            });
            var solutionDetails = await SolutionDetailEntity.FetchAllAsync();
            solutionDetails.Select(m => new
            {
                Solution = m.SolutionId,
                m.AboutUrl,
                m.Features,
                m.Summary,
                m.FullDescription,
                ClientApplication = string.IsNullOrWhiteSpace(m.ClientApplication) ? null : JToken.Parse(m.ClientApplication).ToString()
            }).Should().BeEquivalentTo(expectedSolutionDetails);
        }

        [Then(@"Last Updated has updated on the SolutionDetail for solution (.*)")]
        public async Task LastUpdatedHasUpdatedOnSolutionDetail(string solutionId)
        {
            var contacts = await SolutionDetailEntity.GetBySolutionIdAsync(solutionId);

            var lastUpdated = contacts.LastUpdated;

            var currentDateTime = DateTime.Now;
            var pastDateTime = currentDateTime.AddSeconds(-5);

            lastUpdated.Should().BeOnOrAfter(pastDateTime);
            lastUpdated.Should().BeOnOrBefore(currentDateTime);
        }

        private class SolutionDetailTable
        {
            public string Solution { get; set; }

            public string SummaryDescription { get; set; }

            public string FullDescription { get; set; }

            public string AboutUrl { get; set; }

            public string Features { get; set; }

            public string ClientApplication { get; set; }
        }
    }
}
