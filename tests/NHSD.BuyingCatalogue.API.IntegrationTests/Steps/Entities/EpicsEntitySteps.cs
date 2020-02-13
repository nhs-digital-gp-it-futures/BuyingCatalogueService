using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    public sealed class EpicsEntitySteps
    {
        [Given(@"Epics exist")]
        public static async Task GivenEpicsExist(Table table)
        {
            foreach (var epic in table.CreateSet<EpicsTable>())
            {
                await InsertEpicsAsync(epic).ConfigureAwait(false);
            }
        }

        [Given("Solutions are linked to Epics")]
        public static async Task GivenSolutionsAreLinkedToEpics(Table table)
        {
            var epics = (await EpicEntity.FetchAllAsync().ConfigureAwait(false)).ToDictionary(e=>e.Id);

            foreach (var solutionEpicTable in table.CreateSet<SolutionClaimedEpicTable>().Where(set=>set.EpicIds.Any()))
            {
                if (!Enum.TryParse(solutionEpicTable.Status, out SolutionEpicEntityBuilder.SolutionEpicStatus status))
                    status = SolutionEpicEntityBuilder.SolutionEpicStatus.Passed;

                foreach (var epicId in solutionEpicTable.EpicIds)
                {
                    await SolutionEpicEntityBuilder.Create()
                        .WithSolutionId(solutionEpicTable.SolutionId)
                        .WithCapabilityId(epics[epicId].CapabilityId)
                        .WithEpicId(epicId)
                        .WithStatus(status)
                        .Build()
                        .InsertAsync()
                        .ConfigureAwait(false);
                }
            
            }
        }

        [Then(@"Solutions claim only these Epics")]
        public static async Task ThenSolutionsAreLinkedToEpics(Table table)
        {
            foreach (var row in table.CreateSet<SolutionClaimedEpicTable>())
            {
                var epics = await SolutionEpicEntity.FetchAllEpicIdsForSolutionAsync(row.SolutionId)
                    .ConfigureAwait(false);

                epics.Should().BeEquivalentTo(row.EpicIds);
            }
        }

        private static async Task InsertEpicsAsync(EpicsTable epicTable)
        {
            var capabilities = await CapabilityEntity.FetchAllAsync().ConfigureAwait(false);

            var capId = capabilities.First(x =>
                string.Equals(x.CapabilityRef, epicTable.CapabilityRef, StringComparison.OrdinalIgnoreCase)).Id;

            if (!Enum.TryParse(epicTable.CompliancyLevel, out EpicEntityBuilder.CompliancyLevel compliancyLevel))
                compliancyLevel = EpicEntityBuilder.CompliancyLevel.Must;

            var epic = EpicEntityBuilder.Create()
                .WithId(epicTable.Id)
                .WithCapabilityId(capId)
                .WithName(string.IsNullOrWhiteSpace(epicTable.Name) ? $"Name {epicTable.Id}" : epicTable.Name)
                .WithCompliancyLevel(compliancyLevel)
                .WithActive(epicTable.Active)
                .Build();

            await epic.InsertAsync().ConfigureAwait(false);
        }

        private class EpicsTable
        {
            public string Id { get; set; }

            public string CapabilityRef { get; set; }

            public string Name { get; set; }
            public string CompliancyLevel { get; set; }
            public bool Active { get; set; } = true;
        }

        private class SolutionClaimedEpicTable
        {
            public string SolutionId { get; set; }

            public List<string> EpicIds { get; set; }

            public string Status { get; set; }
        }
    }
}
