using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    internal sealed class SolutionDetailEntitySteps
    {
        [Given(@"SolutionDetail exist")]
        public static async Task GivenSolutionDetailExist(Table table)
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
                    .WithHosting(solutionDetail.Hosting)
                    .WithRoadMap(solutionDetail.RoadMap)
                    .WithIntegrationsUrl(solutionDetail.IntegrationsUrl)
                    .WithImplementationTimescales(solutionDetail.ImplementationDetail)
                    .WithLastUpdated(solutionDetail.LastUpdated != DateTime.MinValue ? solutionDetail.LastUpdated : DateTime.UtcNow)
                    .Build()
                    .InsertAndSetCurrentForSolutionAsync();
            }
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
                RoadMap = string.IsNullOrWhiteSpace(m.RoadMap) ? null : m.RoadMap,
                IntegrationsUrl = string.IsNullOrWhiteSpace(m.IntegrationsUrl) ? null : m.IntegrationsUrl,
                ImplementationDetail = string.IsNullOrWhiteSpace(m.ImplementationDetail) ? null : m.ImplementationDetail,
                ClientApplication = string.IsNullOrWhiteSpace(m.ClientApplication) ? null : JToken.Parse(m.ClientApplication).ToString(),
                Hosting = string.IsNullOrWhiteSpace(m.Hosting) ? null : JToken.Parse(m.Hosting).ToString(),
            });

            var solutionDetails = await SolutionDetailEntity.FetchAllAsync();

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
                Hosting = string.IsNullOrWhiteSpace(m.Hosting) ? null : JToken.Parse(m.Hosting).ToString(),
            }).Should().BeEquivalentTo(expectedSolutionDetails);
        }

        [Then(@"Last Updated has updated on the SolutionDetail for solution (.*)")]
        public static async Task LastUpdatedHasUpdatedOnSolutionDetail(string solutionId)
        {
            var solutionDetail = await SolutionDetailEntity.GetBySolutionIdAsync(solutionId);
            (await solutionDetail.LastUpdated.SecondsFromNow()).Should().BeLessOrEqualTo(5);
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class SolutionDetailTable
        {
            public string Solution { get; init; }

            public string SummaryDescription { get; init; }

            public string FullDescription { get; init; }

            public string AboutUrl { get; init; }

            public string Features { get; init; }

            public string ClientApplication { get; init; }

            public string Hosting { get; init; }

            public string RoadMap { get; init; }

            public string IntegrationsUrl { get; init; }

            public string ImplementationDetail { get; init; }

            public DateTime LastUpdated { get; init; }
        }
    }
}
