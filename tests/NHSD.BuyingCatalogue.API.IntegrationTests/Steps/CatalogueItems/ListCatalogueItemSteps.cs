using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl;
using JetBrains.Annotations;
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
        private readonly Response response;
        private readonly ScenarioContext context;

        public ListCatalogueItemSteps(Response response, ScenarioContext context)
        {
            this.response = response ?? throw new ArgumentNullException(nameof(response));
            this.context = context;
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
                    .WithPublishedStatusId((int)catalogueItem.PublishedStatus)
                    .Build();

                catalogueItemList.Add(item);
                await item.InsertAsync();
            }

            context[ScenarioContextKeys.CatalogueItems] = catalogueItemList;
        }

        [When(@"a Get request is made to retrieve a list of catalogue items with supplierId (.*) and catalogueItemType (.*) and publishedStatus (.*)")]
        public async Task WhenAGetRequestIsMadeToRetrieveAListOfCatalogueItemsWithSupplierIdAndCatalogueItemType(
            string supplierId,
            string catalogueItemType,
            string publishedStatus)
        {
            var url = new Url("http://localhost:5200/api/v1/catalogue-items");

            if (supplierId != null)
                url.QueryParams.Add(nameof(supplierId), supplierId);

            if (catalogueItemType != null)
                url.QueryParams.Add(nameof(catalogueItemType), catalogueItemType);

            if (publishedStatus != null)
                url.QueryParams.Add(nameof(publishedStatus), publishedStatus);

            response.Result = await Client.GetAsync(url.ToString());
        }

        [Then(@"the response contains a list of catalogue item details filtered by (.*) and (.*) and (.*)")]
        public async Task ThenTheResponseContainsAListOfCatalogueItemDetailsFilteredByAnd(
            string supplierId,
            CatalogueItemType? catalogueItemType,
            PublishedStatus? publishedStatus)
        {
            IEnumerable<CatalogueItemEntity> expectedCatalogueItems =
                (context[ScenarioContextKeys.CatalogueItems] as IEnumerable<CatalogueItemEntity>)?.ToList();

            bool MatchesSupplier(CatalogueItemEntity item) => supplierId is null || item.SupplierId == supplierId;
            bool MatchesCatalogueItemType(CatalogueItemEntity item) => catalogueItemType is null || item.CatalogueItemTypeId == (int)catalogueItemType;
            bool MatchesPublishedStatus(CatalogueItemEntity item) => publishedStatus is null || item.PublishedStatusId == (int)publishedStatus;
            bool MatchesFilters(CatalogueItemEntity item) => MatchesSupplier(item)
                && MatchesCatalogueItemType(item)
                && MatchesPublishedStatus(item);

            var filteredExpected = expectedCatalogueItems?.Where(MatchesFilters);

            var content = await response.ReadBody();

            var actual = content.Select(catalogueItem => new CatalogueItemResponseTable
            {
                CatalogueItemId = catalogueItem.Value<string>("catalogueItemId"),
                Name = catalogueItem.Value<string>("name"),
            });

            var expected = filteredExpected?.Select(c => new CatalogueItemResponseTable
            {
                CatalogueItemId = c.CatalogueItemId,
                Name = c.Name,
            });

            actual.Should().BeEquivalentTo(expected);
        }

        [Then(@"an empty catalogue items list is returned")]
        public async Task ThenAnEmptySolutionIsReturned()
        {
            var catalogueItem = await response.ReadBody();
            catalogueItem.Count().Should().Be(0);
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class CatalogueItemTable
        {
            public string CatalogueItemId { get; init; }

            public string Name { get; init; }

            public CatalogueItemType CatalogueItemType { get; init; }

            public string SupplierId { get; init; }

            public PublishedStatus PublishedStatus { get; init; }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class CatalogueItemResponseTable
        {
            public string CatalogueItemId { get; init; }

            public string Name { get; init; }
        }
    }
}
