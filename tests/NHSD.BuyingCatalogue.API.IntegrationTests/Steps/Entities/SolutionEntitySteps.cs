using System;
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
        [Given(@"Solutions exist")]
        public static async Task GivenSolutionsExist(Table table)
        {
            foreach (var solutionTable in table.CreateSet<SolutionTable>())
            {
                await SolutionEntityBuilder.Create()
                    .WithName(solutionTable.SolutionName)
                    .WithId(solutionTable.SolutionID)
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
            var solutions = await SolutionEntity.FetchAllAsync().ConfigureAwait(false);
            var capabilities = await CapabilityEntity.FetchAllAsync().ConfigureAwait(false);

            foreach (var solutionCapabilityTable in table.CreateSet<SolutionCapabilityTable>())
            {
                await SolutionCapabilityEntityBuilder.Create()
                    .WithSolutionId(solutions.First(s => s.Name == solutionCapabilityTable.Solution).Id)
                    .WithCapabilityId(capabilities.First(s => s.Name == solutionCapabilityTable.Capability).Id)
                    .Build()
                    .InsertAsync()
                    .ConfigureAwait(false);
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
                SolutionID = s.Id,
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
            public string SolutionID { get; set; }

            public string SolutionName { get; set; }

            public int SupplierStatusId { get; set; }

            public string SupplierId { get; set; }

            public int PublishedStatusId { get; set; } = 3;

            public DateTime LastUpdated { get; set; }
        }

        private class SolutionCapabilityTable
        {
            public string Solution { get; set; }

            public string Capability { get; set; }
        }

        private class SolutionUpdatedTable
        {
            public string SolutionID { get; set; }

            public string SolutionName { get; set; }
        }
    }
}
