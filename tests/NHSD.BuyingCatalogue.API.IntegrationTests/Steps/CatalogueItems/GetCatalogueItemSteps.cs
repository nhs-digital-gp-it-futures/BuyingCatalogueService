using System;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.CatalogueItems
{
    [Binding]
    internal sealed class GetCatalogueItemSteps
    {
        private const string GetCatalogueItemUrlTemplate = "http://localhost:5200/api/v1/catalogue-items";

        private readonly Response response;

        public GetCatalogueItemSteps(Response response)
        {
            this.response = response ?? throw new ArgumentNullException(nameof(response));
        }

        [When(@"a GET request is made to retrieve a catalogue item with ID '(.*)'")]
        public async Task WhenAGetRequestIsMadeToRetrieveACatalogueItemWithId(string catalogueItemId) =>
            response.Result = await Client.GetAsync(GetCatalogueItemUrl(catalogueItemId));

        [Then(@"the response contains the catalogue item details")]
        public async Task ThenTheResponseContainsTheCatalogueItemDetails(Table table)
        {
            var expectedCatalogueItem = table.CreateInstance<GetCatalogueItemResponseTable>();
            var content = await response.ReadBody();

            var actualCatalogueItem = new GetCatalogueItemResponseTable
            {
                CatalogueItemId = content.Value<string>("catalogueItemId"),
                Name = content.Value<string>("name"),
            };

            actualCatalogueItem.Should().BeEquivalentTo(expectedCatalogueItem);
        }

        private static string GetCatalogueItemUrl(string catalogueItemId) =>
            $"{GetCatalogueItemUrlTemplate}/{catalogueItemId}";

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class GetCatalogueItemResponseTable
        {
            public string CatalogueItemId { get; init; }

            public string Name { get; init; }
        }
    }
}
