using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    public sealed class CapabilityEntitySteps
    {
        [Given(@"Capabilities exist")]
        public static async Task GivenCapabilitiesExist(Table table)
        {
            foreach (var capability in table.CreateSet<CapabilityTable>())
            {
                await InsertCapabilityAsync(capability).ConfigureAwait(false);
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
            await capability.InsertAsync().ConfigureAwait(false);

            await FrameworkCapabilitiesEntityBuilder.Create()
                .WithCapabilityId(capability.Id)
                .WithIsFoundation(capabilityTable.IsFoundation)
                .Build()
                .InsertAsync().ConfigureAwait(false);
        }


        private class CapabilityTable
        {
            public string CapabilityName { get; set; }

            public string CapabilityRef { get; set; } = "Ref";

            public bool IsFoundation { get; set; }

            public string Version { get; set; } = "1.0";

            public string Description { get; set; } = "Capability Description";

            public string SourceUrl { get; set; } = "http://source.url";
        }
    }
}
