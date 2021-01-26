using System.Threading.Tasks;
using JetBrains.Annotations;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    internal sealed class CapabilityEntitySteps
    {
        [Given(@"Capabilities exist")]
        public static async Task GivenCapabilitiesExist(Table table)
        {
            foreach (var capability in table.CreateSet<CapabilityTable>())
            {
                await InsertCapabilityAsync(capability);
            }
        }

        private static async Task InsertCapabilityAsync(CapabilityTable capabilityTable)
        {
            var capability = CapabilityEntityBuilder.Create()
                .WithName(capabilityTable.CapabilityName)
                .WithCapabilityRef(capabilityTable.CapabilityRef)
                .WithVersion(capabilityTable.Version)
                .WithDescription(capabilityTable.Description)
                .WithSourceUrl(capabilityTable.SourceUrl)
                .Build();
            await capability.InsertAsync();

            await FrameworkCapabilitiesEntityBuilder.Create()
                .WithCapabilityId(capability.Id)
                .WithIsFoundation(capabilityTable.IsFoundation)
                .Build()
                .InsertAsync();
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private class CapabilityTable
        {
            public string CapabilityName { get; init; }

            public string CapabilityRef { get; init; } = "Ref";

            public bool IsFoundation { get; init; }

            public string Version { get; init; } = "1.0";

            public string Description { get; init; } = "Capability Description";

            public string SourceUrl { get; init; } = "http://source.url";
        }
    }
}
