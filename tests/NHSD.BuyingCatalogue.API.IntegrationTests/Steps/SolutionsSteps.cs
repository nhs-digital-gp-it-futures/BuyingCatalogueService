using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    public sealed class SolutionsSteps
    {
        private readonly ScenarioContext _context;

        public SolutionsSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Given(@"Capabilities exist")]
        public void GivenCapabilitiesExist(Table table)
        {
            table.CreateSet<CapabilityTable>().ToList().ForEach(async c => await InsertCapabilityAsync(c));
        }

        private async Task InsertCapabilityAsync(CapabilityTable capabilityTable)
        {
            var capability = CapabilityEntityBuilder.Create().WithName(capabilityTable.CapabilityName).Build();
            await FrameworkCapabilitiesEntityBuilder.Create().WithCapabilityId(capability.Id).WithIsFoundation(capabilityTable.IsFoundation).Build().InsertAsync();
            await capability.InsertAsync();
        }

        [Given(@"Organisations exist")]
        public void GivenOrganisationsExist(Table table)
        {
            _context.Pending();
        }

        [Given(@"Solutions exist")]
        public void GivenSolutionsExist(Table table)
        {
            _context.Pending();
        }

        [Given(@"Solutions are linked to Capabilities")]
        public void GivenSolutionsAreLinkedToCapabilities(Table table)
        {
            _context.Pending();
        }

        [Given(@"a request containing no selection criteria")]
        public void GivenARequestContainingNoSelectionCriteria()
        {
            GivenARequestContainingTheCapabilities(new List<string>());
        }

        [Given(@"a request containing a single capability (\S+)")]
        public void GivenARequestContainingASingleCapability(string capability)
        {
            GivenARequestContainingTheCapabilities(new List<string> { capability });
        }

        [Given(@"a request containing the capabilities (.*)")]
        public void GivenARequestContainingTheCapabilities(List<string> capabilities)
        {
            _context.Pending();
        }

        [Then(@"the solutions (.*) are found in the response")]
        public void ThenTheSolutionsAreFoundInTheResponse(List<string> solutions)
        {
            _context.Pending();
        }

        [Then(@"the solutions (.*) are not found in the response")]
        public void ThenTheSolutionsAreNotFoundInTheResponse(List<string> solutions)
        {
            _context.Pending();
        }

        [Then(@"the details of the solutions returned are as follows")]
        public void ThenTheDetailsOfTheSolutionsReturnedAreAsFollows(Table table)
        {
            _context.Pending();
        }

        [StepArgumentTransformation]
        public List<string> TransformToListOfString(string commaSeparatedList)
        {
            return commaSeparatedList.Split(",").ToList();
        }

        private class CapabilityTable
        {
            public string CapabilityName { get; set; }

            public bool IsFoundation { get; set; }
        }
    }
}
