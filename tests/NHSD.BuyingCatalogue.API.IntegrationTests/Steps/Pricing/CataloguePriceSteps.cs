using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Pricing
{
    [Binding]
    internal sealed class CataloguePriceSteps
    {
        private readonly Response _response;
        private readonly ScenarioContext _context;

        private const string priceToken = "prices";
        private const string getPriceBySolutionIdUrlTemplate = "http://localhost:5200/api/v1/solutions/{0}/prices";
        private const string getPricesUrl = "http://localhost:5200/api/v1/prices";
        private readonly string getPricesByCatalogueItemIdUrlTemplate = $"{getPricesUrl}?catalogueItemId={{0}}";
        private readonly string getPricesByPriceIdUrlTemplate = $"{getPricesUrl}/{{0}}";

        public CataloguePriceSteps(Response response, ScenarioContext context)
        {
            _response = response;
            _context = context;
        }

        [Given(@"CataloguePrice exists")]
        public async Task GivenCataloguePriceExists(Table table)
        {
            IDictionary<int, int> cataloguePriceDictionary = new Dictionary<int, int>();

            IDictionary<string, int> catalogueItemIdPriceIdDictionary = new Dictionary<string, int>();

            foreach (var cataloguePrice in table.CreateSet<CataloguePriceTable>())
            {
                var price = CataloguePriceEntityBuilder.Create()
                    .WithCatalogueItemId(cataloguePrice.CatalogueItemId)
                    .WithPriceTypeId((int)cataloguePrice.CataloguePriceTypeEnum)
                    .WithProvisioningTypeId((int)cataloguePrice.ProvisioningTypeEnum)
                    .WithCurrencyCode(cataloguePrice.CurrencyCode)
                    .WithPrice(cataloguePrice.Price)
                    .WithPricingUnitId(cataloguePrice.PricingUnitId)
                    .WithTimeUnit((int)cataloguePrice.TimeUnitEnum == -1 ? (int?)null : (int)cataloguePrice.TimeUnitEnum)
                    .Build();

                var cataloguePriceId = await price.InsertAsync<int>();

                if (cataloguePrice.CataloguePriceTierRef != null)
                    cataloguePriceDictionary.Add((int)cataloguePrice.CataloguePriceTierRef, cataloguePriceId);

                if (!string.IsNullOrEmpty(cataloguePrice.CataloguePriceIdRef))
                    catalogueItemIdPriceIdDictionary.Add(cataloguePrice.CataloguePriceIdRef, cataloguePriceId);
            }

            _context[ScenarioContextKeys.CatalogueTierMapDictionary] = cataloguePriceDictionary;

            _context[ScenarioContextKeys.CataloguePriceIdMapDictionary] = catalogueItemIdPriceIdDictionary;
        }

        [When(@"a GET request is made to retrieve the pricing with Solution ID (.*)")]
        public async Task WhenAGetRequestIsMadeToRetrieveThePricingWithSolutionID(string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(CultureInfo.InvariantCulture, getPriceBySolutionIdUrlTemplate, solutionId));
        }

        [When(@"a GET request is made to retrieve the list of prices using catalogue item ID (.*)")]
        public async Task WhenAGetRequestIsMadeToRetrieveThePricingByCatalogueItemId(string catalogueItemId)
        {
            _response.Result = await Client.GetAsync(string.Format(CultureInfo.InvariantCulture, getPricesByCatalogueItemIdUrlTemplate, catalogueItemId));
        }

        [When(@"a GET request is made to retrieve the list of prices")]
        public async Task WhenAGetRequestIsMadeToRetrieveTheListOfPrices()
        {
            _response.Result = await Client.GetAsync(getPricesUrl);
        }

        [When(@"a GET request is made to retrieve a single price using the PriceId associated with CaltaloguePriceIdRef (.*)")]
        public async Task WhenAGetRequestIsMadeToRetrieveThePriceUsingPriceIdAssociatedWithSolutionId(string caltaloguePriceIdRef)
        {
            var cataloguePriceId = _context.GetCataloguePriceIdsByCatalougePriceIdRef(caltaloguePriceIdRef);
            _response.Result = await Client.GetAsync(string.Format(CultureInfo.InvariantCulture, getPricesByPriceIdUrlTemplate, cataloguePriceId));
        }

        [Then(@"Prices are returned")]
        public async Task ThenPricesAreReturned(Table table)
        {
            var expected = table.CreateSet<PriceResultTable>().ToList();
            const string itemUnitToken = "itemUnit";
            const string timeUnitToken = "timeUnit";

            var content = (await _response.ReadBody()).SelectToken(priceToken).Select(x => new
            {
                Type = x.Value<string>("type"),
                ProvisioningType = x.Value<string>("provisioningType"),
                CurrencyCode = x.Value<string>("currencyCode"),
                Price = x.Value<decimal?>("price"),
                PricingItemName = x.SelectToken(itemUnitToken).Value<string>("name"),
                PricingItemDescription = x.SelectToken(itemUnitToken).Value<string>("description"),
                PricingItemTierName = x.SelectToken(itemUnitToken).Value<string>("tierName"),
                TimeUnitName = x.SelectToken(timeUnitToken)?.Value<string>("name"),
                TimeUnitDescription = x.SelectToken(timeUnitToken)?.Value<string>("description")
            });

            content.Should().BeEquivalentTo(expected);
        }

        [Then(@"an empty price list is returned")]
        public async Task ThenAnEmptyListIsReturned()
        {
            var response = (await _response.ReadBody()).SelectToken(priceToken).ToList();
            response.Should().BeEmpty();
        }

        [Then(@"a Price is returned")]
        public async Task ThenPriceIsReturned(Table table)
        {
            var expected = table.CreateSet<PriceResultTable>().Single();
            const string itemUnitToken = "itemUnit";
            const string timeUnitToken = "timeUnit";

            var response = await _response.ReadBody();
            var content = new
            {
                Type = response.Value<string>("type"),
                CurrencyCode = response.Value<string>("currencyCode"),
                Price = response.Value<decimal?>("price"),
                ProvisioningType = response.Value<string>("provisioningType"),
                PricingItemName = response.SelectToken(itemUnitToken).Value<string>("name"),
                PricingItemDescription = response.SelectToken(itemUnitToken).Value<string>("description"),
                PricingItemTierName = response.SelectToken(itemUnitToken).Value<string>("tierName"),
                TimeUnitName = response.SelectToken(timeUnitToken)?.Value<string>("name"),
                TimeUnitDescription = response.SelectToken(timeUnitToken)?.Value<string>("description")
            };

            content.Should().BeEquivalentTo(expected);
        }

        [Then(@"the Prices Tiers are returned")]
        public async Task ThenThePricesTiersAreReturned(Table table)
        {
            var expectedTable = table.CreateSet<TierTable>().ToList();
            var listExpected = expectedTable.GroupBy(x => x.Section);

            var expected = listExpected.Select(x => x.Select(y => new
            {
                y.Start,
                y.End,
                y.Price
            }));

            var pricesToken = (await _response.ReadBody()).SelectToken(priceToken);

            const string tierToken = "tiers";
            var tierPrices = pricesToken.Where(x => x.SelectToken(tierToken) != null);
            var content = tierPrices.Select(x => new
            {
                Tier = x.SelectToken(tierToken).Select(z => new
                {
                    Start = z.Value<int>("start"),
                    End = z.Value<int?>("end"),
                    Price = z.Value<decimal>("price")
                })
            });

            content.Select(x => x.Tier).Should().BeEquivalentTo(expected, x => x.WithoutStrictOrdering());
        }

        [Then(@"the Price Tiers are returned")]
        public async Task ThenThePriceTiersAreReturned(Table table)
        {
            var expectedTable = table.CreateSet<TierTable>().ToList();

            var expected = expectedTable.Select(y => new
            {
                y.Start,
                y.End,
                y.Price
            });

            var pricesToken = await _response.ReadBody();
            var tierPrices = pricesToken.SelectToken("tiers");

            var content = tierPrices.Select(x => new
            {
                Start = x.Value<int>("start"),
                End = x.Value<int?>("end"),
                Price = x.Value<decimal>("price")
            });

            content.Should().BeEquivalentTo(expected, x => x.WithoutStrictOrdering());
        }

        private sealed class CataloguePriceTable
        {
            public string CatalogueItemId { get; set; }
            public CataloguePriceTypeEnum CataloguePriceTypeEnum { get; set; }
            public ProvisioningTypeEnum ProvisioningTypeEnum { get; set; }
            public string CurrencyCode { get; set; }
            public decimal? Price { get; set; }
            public Guid PricingUnitId { get; set; }
            public TimeUnitEnum? TimeUnitEnum { get; set; }
            public int? CataloguePriceTierRef { get; set; }
            public string CataloguePriceIdRef { get; set; }
        }

        private sealed class PriceResultTable
        {
            public string Type { get; set; }
            public string CurrencyCode { get; set; }
            public decimal? Price { get; set; }
            public string PricingItemName { get; set; }
            public string PricingItemDescription { get; set; }
            public string PricingItemTierName { get; set; }
            public string TimeUnitName { get; set; }
            public string TimeUnitDescription { get; set; }
            public string ProvisioningType { get; set; }
        }

        private sealed class TierTable
        {
            public int? Start { get; set; }
            public int? End { get; set; }
            public decimal? Price { get; set; }
            public int? Section { get; set; }
        }

        private enum CataloguePriceTypeEnum
        {
            Flat = 1,
            Tiered = 2
        }

        private enum ProvisioningTypeEnum
        {
            Patient = 1,
            Declarative = 2,
            OnDemand = 3
        }

        private enum TimeUnitEnum
        {
            Month = 1,
            Year = 2,
            NULL = -1
        }
    }
}
