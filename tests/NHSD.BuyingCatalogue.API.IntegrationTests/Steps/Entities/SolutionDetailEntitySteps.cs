using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Testing.Data;
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
        public static async Task GivenSolutionDetailExist(Table table)
        {
            //foreach (var solutionDetail in table.CreateSet<SolutionDetailTable>())
            //{
            //    await SolutionDetailEntityBuilder.Create()
            //        .WithFeatures(solutionDetail.Features)
            //        .WithSummary(solutionDetail.SummaryDescription)
            //        .WithFullDescription(solutionDetail.FullDescription)
            //        .WithAboutUrl(solutionDetail.AboutUrl)
            //        .WithSolutionId(solutionDetail.Solution)
            //        .WithClientApplication(solutionDetail.ClientApplication)
            //        .WithHosting(solutionDetail.Hosting)
            //        .WithRoadMap(solutionDetail.RoadMap)
            //        .WithIntegrationsUrl(solutionDetail.IntegrationsUrl)
            //        .WithImplementationTimescales(solutionDetail.ImplementationDetail)
            //        .WithLastUpdated(solutionDetail.LastUpdated != DateTime.MinValue ? solutionDetail.LastUpdated : DateTime.UtcNow)
            //        .Build()
            //        .InsertAndSetCurrentForSolutionAsync()
            //        .ConfigureAwait(false);
            //}
        }

        [Given(@"a SolutionDetail (.*) does not exist")]
        public static async Task GivenASolutionDetailDoesNotExist(string solutionId)
        {
            var solutionDetailList = await SolutionDetailEntity.FetchAllAsync().ConfigureAwait(false);
            solutionDetailList.Select(x => x.SolutionId).Should().NotContain(solutionId);
        }

        [Then(@"SolutionDetail exist")]
        public static async Task ThenSolutionDetailExist(Table table)
        {
            var expectedSolutionDetails = table.CreateSet<SolutionDetailTable>().Select(m => new
            {
                m.Solution,
                AboutUrl = string.IsNullOrWhiteSpace(m.AboutUrl) ? null : m.AboutUrl,
                Features = string.IsNullOrWhiteSpace(m.Features) ? null : m.Features,
                Summary = string.IsNullOrWhiteSpace(m.SummaryDescription) ? null : m.SummaryDescription,
                FullDescription = string.IsNullOrWhiteSpace(m.FullDescription) ? null : m.FullDescription,
                ClientApplication = string.IsNullOrWhiteSpace(m.ClientApplication) ? null : JToken.Parse(m.ClientApplication).ToString(),
                Hosting = string.IsNullOrWhiteSpace(m.Hosting) ? null : JToken.Parse(m.Hosting).ToString(),
                RoadMap = string.IsNullOrWhiteSpace(m.RoadMap) ? null : m.RoadMap,
                IntegrationsUrl = string.IsNullOrWhiteSpace(m.IntegrationsUrl) ? null : m.IntegrationsUrl,
                ImplementationDetail = string.IsNullOrWhiteSpace(m.ImplementationDetail) ? null : m.ImplementationDetail,
            });
            var solutionDetails = await SolutionDetailEntity.FetchAllAsync().ConfigureAwait(false);
            solutionDetails.Select(m => new
            {
                Solution = m.SolutionId,
                m.AboutUrl,
                m.Features,
                m.Summary,
                m.FullDescription,
                m.RoadMap,
                m.IntegrationsUrl,
                m.ImplementationDetail,
                ClientApplication = string.IsNullOrWhiteSpace(m.ClientApplication) ? null : JToken.Parse(m.ClientApplication).ToString(),
                Hosting = string.IsNullOrWhiteSpace(m.Hosting) ? null : JToken.Parse(m.Hosting).ToString()
            }).Should().BeEquivalentTo(expectedSolutionDetails);
        }

        [Then(@"Last Updated has updated on the SolutionDetail for solution (.*)")]
        public static async Task LastUpdatedHasUpdatedOnSolutionDetail(string solutionId)
        {
            var solutionDetail = await SolutionDetailEntity.GetBySolutionIdAsync(solutionId).ConfigureAwait(false);
            (await solutionDetail.LastUpdated.SecondsFromNow().ConfigureAwait(false)).Should().BeLessOrEqualTo(5);
        }

        private class SolutionDetailTable
        {
            public string Solution { get; set; }

            public string SummaryDescription { get; set; }

            public string FullDescription { get; set; }

            public string AboutUrl { get; set; }

            public string Features { get; set; }

            public string ClientApplication { get; set; }

            public string Hosting { get; set; }

            public string RoadMap { get; set; }

            public string IntegrationsUrl { get; set; }

            public string ImplementationDetail { get; set; }

            public DateTime LastUpdated { get; set; }
        }
    }
}
