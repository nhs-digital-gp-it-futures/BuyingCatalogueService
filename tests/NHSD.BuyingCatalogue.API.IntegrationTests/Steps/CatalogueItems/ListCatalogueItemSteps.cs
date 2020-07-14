using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.CatalogueItems
{
    [Binding]
    internal sealed class ListCatalogueItemSteps
    {
        private string _listCatalogueItemUrlTemplate = "http://localhost:5200/api/v1/catalogue-items";
        private readonly Response _response;
        private readonly ScenarioContext _context;

        public ListCatalogueItemSteps(Response response, ScenarioContext context)
        {
            _response = response ?? throw new ArgumentNullException(nameof(response));
            _context = context;
        }

        [Given(@"CatalogueItems exist")]
        public async Task GivenCatalogueItemsExist(Table table)
        {
            List<CatalogueItemEntity> catalogueItemList = new List<CatalogueItemEntity>();

            foreach (var catalogueItem in table.CreateSet<CatalogueItemTable>())
            {
                var item = CatalogueItemEntityBuilder.Create()
                    .WithCatalogueItemId(catalogueItem.CatalogueItemId)
                    .WithCatalogueItemTypeId((int)catalogueItem.CatalogueItemType)
                    .WithName(catalogueItem.Name)
                    .WithSupplierId(catalogueItem.SupplierId)
                    .Build();

                catalogueItemList.Add(item);
                await item.InsertAsync();
            }

            _context[ScenarioContextKeys.CatalogueItems] = catalogueItemList;
        }

        [When(@"a Get request is made to retrieve a list of catalogue items with supplierId (.*) and catalogueItemType (.*)")]
        public async Task WhenAGetRequestIsMadeToRetrieveAListOfCatalogueItemsWithSupplierIdAndCatalogueItemType(string supplierId, string catalogueItemType)
        {
            if (supplierId != null)
            {
                _listCatalogueItemUrlTemplate += $"?supplierId={supplierId}";

                if (catalogueItemType != null)
                {
                    _listCatalogueItemUrlTemplate += $"&catalogueItemType={catalogueItemType}";
                }
            }
            else if (catalogueItemType != null)
            {
                _listCatalogueItemUrlTemplate += $"?catalogueItemType={catalogueItemType}";
            }

            _response.Result = await Client.GetAsync(_listCatalogueItemUrlTemplate);
        }

        [Then(@"the response contains a list of catalogue item details filtered by (.*) and (.*)")]
        public async Task ThenTheResponseContainsAListOfCatalogueItemDetailsFilteredByAnd(string supplierId, CatalogueItemType? catalogueItemType)
        {
            IEnumerable<CatalogueItemEntity> expectedCatalogueItems =
                (_context[ScenarioContextKeys.CatalogueItems] as IEnumerable<CatalogueItemEntity>)?.ToList();

            var filteredExpected = expectedCatalogueItems?.Where(x => (supplierId is null || x.SupplierId == supplierId) && (catalogueItemType is null || x.CatalogueItemTypeId == (int)catalogueItemType));

            var content = await _response.ReadBody();

            var actual = content.Select(catalogueItem => new CatalogueItemResponseTable
            {
                CatalogueItemId = catalogueItem.Value<string>("catalogueItemId"),
                Name = catalogueItem.Value<string>("name")
            });

            var expected = filteredExpected?.Select(x => new CatalogueItemResponseTable
            {
                CatalogueItemId = x.CatalogueItemId,
                Name = x.Name
            });

            actual.Should().BeEquivalentTo(expected);
        }

        [Then(@"an empty catalogue items list is returned")]
        public async Task ThenAnEmptySolutionIsReturned()
        {
            var catalogueItem = (await _response.ReadBody());
            catalogueItem.Count().Should().Be(0);
        }

        private sealed class CatalogueItemTable
        {
            public string CatalogueItemId { get; set; }
            public string Name { get; set; }
            public CatalogueItemType CatalogueItemType { get; set; }
            public string SupplierId { get; set; }
        }

        private sealed class CatalogueItemResponseTable
        {
            public string CatalogueItemId { get; set; }

            public string Name { get; set; }
        }
    }
}
