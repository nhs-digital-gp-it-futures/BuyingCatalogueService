using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    public sealed class CatalogueItemEntitySteps
    {
        [Given(@"a catalogue item with ID '(.*)' does not exist")]
        public static async Task GivenACatalogueItemWithIdDoesNotExist(string catalogueItemId)
        {
            var catalogueItemList = await CatalogueItemEntity.FetchAllAsync();
            catalogueItemList.Select(x => x.CatalogueItemId).Should().NotContain(catalogueItemId);
        }
    }
}
