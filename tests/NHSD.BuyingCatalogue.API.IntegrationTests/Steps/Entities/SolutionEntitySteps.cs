using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
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
                    .WithImplementationTimescales(solutionTable.ImplementationDetail)
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
                        .WithSolutionId(solutions[solutionCapability.Solution].SolutionId)
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
            solutionList.Select(s => s.SolutionId).Should().NotContain(solutionId);
        }

        [Then(@"Solutions exist")]
        public static async Task ThenSolutionsExist(Table table)
        {
            var expectedSolutions = table.CreateSet<SolutionUpdatedTable>();
            var solutions = await SolutionEntity.FetchAllAsync();
            solutions.Select(s => new
            {
                SolutionId = s.SolutionId,
                SolutionName = s.Name,
            }).Should().BeEquivalentTo(expectedSolutions);
        }

        [Then(@"Last Updated has been updated for solution (.*)")]
        public static async Task LastUpdatedHasBeenUpdatedOnMarketingContact(string solutionId)
        {
            var contact = await SolutionEntity.GetByIdAsync(solutionId);
            (await contact.LastUpdated.SecondsFromNow()).Should().BeLessOrEqualTo(5);
        }


        [Given(@"solutions have the following details")]
        public static async Task GivenSolutionsHaveTheFollowingDetails(Table table)
        {
            foreach (var solutionDetail in table.CreateSet<SolutionTable>())
            {
                await SolutionEntityBuilder.Create()
                    .WithFeatures(solutionDetail.Features)
                    .WithSummary(solutionDetail.SummaryDescription)
                    .WithFullDescription(solutionDetail.FullDescription)
                    .WithAboutUrl(solutionDetail.AboutUrl)
                    .WithId(solutionDetail.SolutionId)
                    .WithClientApplication(solutionDetail.ClientApplication)
                    .WithHosting(solutionDetail.Hosting)
                    .WithRoadMap(solutionDetail.RoadMap)
                    .WithIntegrationsUrl(solutionDetail.IntegrationsUrl)
                    .WithImplementationTimescales(solutionDetail.ImplementationDetail)
                    .WithLastUpdated(solutionDetail.LastUpdated != DateTime.MinValue ? solutionDetail.LastUpdated : DateTime.UtcNow)
                    .Build()
                    .InsertAsync();
            }
        }

        [Then(@"solutions have the following details")]
        public static async Task ThenSolutionsHaveTheFollowingDetails(Table table)
        {
            var expectedSolutionDetails = table.CreateSet<SolutionTable>().Select(m => new
            {
                m.SolutionId,
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

            var solutionDetails = await SolutionEntity.FetchAllAsync();

            solutionDetails.Select(m => new
            {
                m.SolutionId,
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

            public string ImplementationDetail { get; init; }

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
