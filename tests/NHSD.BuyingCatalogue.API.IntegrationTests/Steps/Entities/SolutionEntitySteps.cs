using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    public sealed class SolutionEntitySteps
    {
        private const int PassedSolutionCapabilityStatusId = 1;
        private const int FailedSolutionCapabilityStatusId = 3;

        [Given(@"Solutions exist")]
        public static async Task GivenSolutionsExist(Table table)
        {
            foreach (var solutionTable in table.CreateSet<SolutionTable>())
            {
                await CatalogueItemEntityBuilder.Create()
                    .WithCatalogueItemId(solutionTable.SolutionId)
                    .WithName(solutionTable.SolutionName ?? "SomeName")
                    .WithSupplierId(solutionTable.SupplierId ?? "Sup 1")
                    .WithPublishedStatusId((int)solutionTable.PublishedStatus)
                    .Build()
                    .InsertAsync();

                await SolutionEntityBuilder.Create()
                    .WithId(solutionTable.SolutionId)
                    .WithFeatures(solutionTable.Features)
                    .WithSummary(solutionTable.SummaryDescription)
                    .WithFullDescription(solutionTable.FullDescription)
                    .WithAboutUrl(solutionTable.AboutUrl)
                    .WithClientApplication(solutionTable.ClientApplication)
                    .WithHosting(solutionTable.Hosting)
                    .WithRoadMap(solutionTable.RoadMap)
                    .WithIntegrationsUrl(solutionTable.IntegrationsUrl)
                    .WithOnLastUpdated(solutionTable.LastUpdated != DateTime.MinValue ? solutionTable.LastUpdated : DateTime.UtcNow)
                    .Build()
                    .InsertAsync();
            }
        }

        [Given(@"Solutions are linked to Capabilities")]
        public static async Task GivenSolutionsAreLinkedToCapabilities(Table table)
        {
            var solutions = (await SolutionEntity.FetchAllAsync()).ToDictionary(s => s.Id);
            var capabilities = (await CapabilityEntity.FetchAllAsync()).ToDictionary(c => c.Name);

            foreach (var solutionCapability in table.CreateSet<SolutionCapabilityTable>())
            {
                if (!solutionCapability.Capability.Any())
                    continue;

                foreach (var capability in solutionCapability.Capability)
                {
                    await SolutionCapabilityEntityBuilder.Create()
                        .WithSolutionId(solutions[solutionCapability.Solution].Id)
                        .WithCapabilityId(capabilities[capability].Id)
                        .WithStatusId(solutionCapability.Pass ? PassedSolutionCapabilityStatusId : FailedSolutionCapabilityStatusId)
                        .Build()
                        .InsertAsync();
                }
            }
        }

        [Then(@"Solutions are linked to Capabilities")]
        public static async Task ThenSolutionsAreLinkedToCapabilities(Table table)
        {
            foreach (var row in table.CreateSet<SolutionCapabilityReferenceTable>())
            {
                var capabilities = await SolutionCapabilityEntity.FetchForSolutionAsync(row.SolutionId);
                capabilities.Should().BeEquivalentTo(row.CapabilityRefs);
            }
        }

        [Given(@"a Solution (.*) does not exist")]
        public static async Task GivenASolutionSlnDoesNotExist(string solutionId)
        {
            var solutionList = await SolutionEntity.FetchAllAsync();
            solutionList.Select(x => x.Id).Should().NotContain(solutionId);
        }

        [Then(@"Solutions exist")]
        public static async Task ThenSolutionsExist(Table table)
        {
            var expectedSolutions = table.CreateSet<SolutionUpdatedTable>();
            var solutions = await SolutionEntity.FetchAllAsync();
            solutions.Select(s => new
            {
                SolutionId = s.Id,
                SolutionName = s.Name,
            }).Should().BeEquivalentTo(expectedSolutions);
        }

        [Then(@"Last Updated has updated on the SolutionEntity for solution (.*)")]
        public static async Task LastUpdatedHasUpdatedOnMarketingContact(string solutionId)
        {
            var contact = await SolutionEntity.GetByIdAsync(solutionId);
            (await contact.LastUpdated.SecondsFromNow()).Should().BeLessOrEqualTo(5);
        }

        private class SolutionTable
        {
            public string SolutionId { get; set; }

            public string SupplierId { get; set; }

            public string SolutionName { get; set; }

            public string Version { get; set; }

            public PublishedStatus PublishedStatus { get; set; } = PublishedStatus.Published;

            public string SummaryDescription { get; set; }

            public string FullDescription { get; set; }

            public string Features { get; set; }

            public string ClientApplication { get; set; }

            public string Hosting { get; set; }

            public string ImplementationDetail { get; set; }

            public string RoadMap { get; set; }

            public string IntegrationsUrl { get; set; }

            public string AboutUrl { get; set; }

            public string ServiceLevelAgreement { get; set; }

            public string WorkOfPlan { get; set; }

            public DateTime LastUpdated { get; set; }
        }

        private class SolutionCapabilityTable
        {
            public string Solution { get; set; }

            public List<string> Capability { get; set; }

            public bool Pass { get; set; } = true;
        }

        private class SolutionUpdatedTable
        {
            public string SolutionId { get; set; }

            public string SolutionName { get; set; }
        }

        private class SolutionCapabilityReferenceTable
        {
            public string SolutionId { get; set; }

            public List<string> CapabilityRefs { get; set; }
        }
    }
}
