using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    internal sealed class SolutionEntitySteps
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
            var solutions = (await SolutionEntity.FetchAllAsync()).ToDictionary(s => s.Name);
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
            solutionList.Select(s => s.Id).Should().NotContain(solutionId);
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

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class SolutionTable
        {
            public string SolutionId { get; init; }

            public string SolutionName { get; init; }

            public string SupplierId { get; init; }

            // TODO: check if set in any features
            public PublishedStatus PublishedStatus { get; init; } = PublishedStatus.Published;

            public string SummaryDescription { get; init; }

            public string FullDescription { get; init; }

            public string Features { get; init; }

            public string ClientApplication { get; init; }

            public string Hosting { get; init; }

            public string RoadMap { get; init; }

            public string IntegrationsUrl { get; init; }

            public string AboutUrl { get; init; }

            public DateTime LastUpdated { get; init; }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class SolutionCapabilityTable
        {
            public string Solution { get; init; }

            public List<string> Capability { get; init; }

            public bool Pass { get; init; } = true;
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class SolutionUpdatedTable
        {
            public string SolutionId { get; init; }

            public string SolutionName { get; init; }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class SolutionCapabilityReferenceTable
        {
            public string SolutionId { get; init; }

            public List<string> CapabilityRefs { get; init; }
        }
    }
}
