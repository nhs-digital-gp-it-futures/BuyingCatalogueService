using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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

        //Constants from Integration Reference Data at /tests/NHSD.BuyingCatalogue.Testing.Data/SqlResources/ReferenceData.sql
        private const int PassedSolutionCapabilityStatusId = 1;
        private const int FailedSolutionCapabilityStatusId = 2;

        [Given(@"Solutions exist")]
        public static async Task GivenSolutionsExist(Table table)
        {
            foreach (var solutionTable in table.CreateSet<SolutionTable>())
            {
                await SolutionEntityBuilder.Create()
                    .WithName(solutionTable.SolutionName)
                    .WithId(solutionTable.SolutionId)
                    .WithOnLastUpdated(solutionTable.LastUpdated)
                    .WithSupplierId(solutionTable.SupplierId)
                    .WithSupplierStatusId(solutionTable.SupplierStatusId)
                    .WithPublishedStatusId(solutionTable.PublishedStatusId)
                    .WithOnLastUpdated(solutionTable.LastUpdated != DateTime.MinValue ? solutionTable.LastUpdated : DateTime.UtcNow)
                    .Build()
                    .InsertAsync()
                    .ConfigureAwait(false);
            }
        }

        [Given(@"Solutions are linked to Capabilities")]
        public static async Task GivenSolutionsAreLinkedToCapabilities(Table table)
        {
            var solutions = (await SolutionEntity.FetchAllAsync().ConfigureAwait(false)).ToDictionary(s=>s.Name);
            var capabilities = (await CapabilityEntity.FetchAllAsync().ConfigureAwait(false)).ToDictionary(c=>c.Name);

            foreach (var solutionCapability in table.CreateSet<SolutionCapabilityTable>())
            {
                if (solutionCapability.Capability.Any())
                {
                    foreach (var capability in solutionCapability.Capability)
                    {
                        await SolutionCapabilityEntityBuilder.Create()
                        .WithSolutionId(solutions[solutionCapability.Solution].Id)
                        .WithCapabilityId(capabilities[capability].Id)
                        .WithStatusId(solutionCapability.Pass ? PassedSolutionCapabilityStatusId : FailedSolutionCapabilityStatusId)
                        .Build()
                        .InsertAsync()
                        .ConfigureAwait(false);
                    }
                }
            }
        }

        [Then(@"Solutions are linked to Capabilities")]
        public static async Task ThenSolutionsAreLinkedToCapabilities(Table table)
        {
            foreach (var row in table.CreateSet<SolutionCapabilityReferenceTable>())
            {
                var capabilities = await SolutionCapabilityEntity.FetchForSolutionAsync(row.SolutionId)
                    .ConfigureAwait(false);

                capabilities.Should().BeEquivalentTo(row.CapabilityRefs);
            }
        }
        
        [Given(@"a Solution (.*) does not exist")]
        public static async Task GivenASolutionSlnDoesNotExist(string solutionId)
        {
            var solutionList = await SolutionEntity.FetchAllAsync().ConfigureAwait(false);
            solutionList.Select(x => x.Id).Should().NotContain(solutionId);
        }

        [Then(@"Solutions exist")]
        public static async Task ThenSolutionsExist(Table table)
        {
            var expectedSolutions = table.CreateSet<SolutionUpdatedTable>();
            var solutions = await SolutionEntity.FetchAllAsync().ConfigureAwait(false);
            solutions.Select(s => new
            {
                SolutionId = s.Id,
                SolutionName = s.Name,
            }).Should().BeEquivalentTo(expectedSolutions);
        }

        [Then(@"the field \[SupplierStatusId] for Solution (.*) should correspond to '(.*)'")]
        public static async Task ThenFieldSolutionSupplierStatusIdShouldCorrespondTo(string solutionId, string supplierStatusName)
        {
            var status = await SolutionSupplierStatusEntity.GetByNameAsync(supplierStatusName).ConfigureAwait(false);
            var solution = await SolutionEntity.GetByIdAsync(solutionId).ConfigureAwait(false);

            solution.SupplierStatusId.Should().Be(status.Id);
        }

        [Then(@"Last Updated has updated on the SolutionEntity for solution (.*)")]
        public static async Task LastUpdatedHasUpdatedOnMarketingContact(string solutionId)
        {
            var contact = await SolutionEntity.GetByIdAsync(solutionId).ConfigureAwait(false);
            (await contact.LastUpdated.SecondsFromNow().ConfigureAwait(false)).Should().BeLessOrEqualTo(5);
        }

        private class SolutionTable
        {
            public string SolutionId { get; set; }

            public string SolutionName { get; set; }

            public int SupplierStatusId { get; set; }

            public string SupplierId { get; set; }

            public int PublishedStatusId { get; set; } = 3;

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
