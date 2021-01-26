using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    internal sealed class EpicsEntitySteps
    {
        [Given(@"Epics exist")]
        public static async Task GivenEpicsExist(Table table)
        {
            foreach (var epic in table.CreateSet<EpicsTable>())
            {
                await InsertEpicsAsync(epic);
            }
        }

        [Given("Solutions are linked to Epics")]
        public static async Task GivenSolutionsAreLinkedToEpics(Table table)
        {
            var epics = (await EpicEntity.FetchAllAsync()).ToDictionary(e => e.Id);

            foreach (var solutionEpicTable in table.CreateSet<SolutionClaimedEpicTable>().Where(set => set.EpicIds.Any()))
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
                        .InsertAsync();
                }
            }
        }

        [Then(@"Solutions claim only these Epics")]
        public static async Task ThenSolutionsAreLinkedToEpics(Table table)
        {
            foreach (var row in table.CreateSet<SolutionClaimedEpicTable>())
            {
                var epics = await SolutionEpicEntity.FetchAllEpicIdsForSolutionAsync(row.SolutionId);

                epics.Should().BeEquivalentTo(row.EpicIds);
            }
        }

        private static async Task InsertEpicsAsync(EpicsTable epicTable)
        {
            var capabilities = await CapabilityEntity.FetchAllAsync();

            var capId = capabilities.First(
                c => string.Equals(c.CapabilityRef, epicTable.CapabilityRef, StringComparison.OrdinalIgnoreCase)).Id;

            if (!Enum.TryParse(epicTable.CompliancyLevel, out EpicEntityBuilder.CompliancyLevel compliancyLevel))
                compliancyLevel = EpicEntityBuilder.CompliancyLevel.Must;

            var epic = EpicEntityBuilder.Create()
                .WithId(epicTable.Id)
                .WithCapabilityId(capId)
                .WithName(string.IsNullOrWhiteSpace(epicTable.Name) ? $"Name {epicTable.Id}" : epicTable.Name)
                .WithCompliancyLevel(compliancyLevel)
                .WithActive(epicTable.Active)
                .Build();

            await epic.InsertAsync();
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class EpicsTable
        {
            public string Id { get; init; }

            public string CapabilityRef { get; init; }

            public string Name { get; init; }

            public string CompliancyLevel { get; init; }

            public bool Active { get; init; } = true;
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class SolutionClaimedEpicTable
        {
            public string SolutionId { get; init; }

            public List<string> EpicIds { get; init; }

            public string Status { get; init; }
        }
    }
}
