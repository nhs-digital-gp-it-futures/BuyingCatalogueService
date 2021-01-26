using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    internal sealed class CatalogueItemEntitySteps
    {
        [Given(@"a catalogue item with ID '(.*)' does not exist")]
        public static async Task GivenACatalogueItemWithIdDoesNotExist(string catalogueItemId)
        {
            var catalogueItemList = await CatalogueItemEntity.FetchAllAsync();
            catalogueItemList.Select(c => c.CatalogueItemId).Should().NotContain(catalogueItemId);
        }
    }
}
