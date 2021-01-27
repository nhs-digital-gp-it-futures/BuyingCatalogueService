using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
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
        private const string PriceToken = "prices";
        private const string GetPricesUrl = "http://localhost:5200/api/v1/prices";

        private readonly Response response;
        private readonly ScenarioContext context;
        private readonly string getPricesByCatalogueItemIdUrlTemplate = $"{GetPricesUrl}?catalogueItemId={{0}}";
        private readonly string getPricesByPriceIdUrlTemplate = $"{GetPricesUrl}/{{0}}";

        public CataloguePriceSteps(Response response, ScenarioContext context)
        {
            this.response = response;
            this.context = context;
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private enum CataloguePriceTypeEnum
        {
            Flat = 1,
            Tiered = 2,
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private enum ProvisioningTypeEnum
        {
            Patient = 1,
            Declarative = 2,
            OnDemand = 3,
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private enum TimeUnit
        {
            Month = 1,
            Year = 2,
            Null = -1,
        }

        [Given(@"CataloguePrice exists")]
        public async Task GivenCataloguePriceExists(Table table)
        {
            IDictionary<int, int> cataloguePriceDictionary = new Dictionary<int, int>();

            IDictionary<string, int> catalogueItemIdPriceIdDictionary = new Dictionary<string, int>();

            foreach (var cataloguePrice in table.CreateSet<CataloguePriceTable>())
            {
                int? timeUnit = null;

                if (cataloguePrice.TimeUnitEnum.HasValue)
                {
                    var enumValue = (int)cataloguePrice.TimeUnitEnum.Value;
                    timeUnit = enumValue == -1 ? (int?)null : enumValue;
                }

                var price = CataloguePriceEntityBuilder.Create()
                    .WithCatalogueItemId(cataloguePrice.CatalogueItemId)
                    .WithPriceTypeId((int)cataloguePrice.CataloguePriceTypeEnum)
                    .WithProvisioningTypeId((int)cataloguePrice.ProvisioningTypeEnum)
                    .WithCurrencyCode(cataloguePrice.CurrencyCode)
                    .WithPrice(cataloguePrice.Price)
                    .WithPricingUnitId(cataloguePrice.PricingUnitId)
                    .WithTimeUnit(timeUnit)
                    .Build();

                var cataloguePriceId = await price.InsertAsync<int>();

                if (cataloguePrice.CataloguePriceTierRef is not null)
                    cataloguePriceDictionary.Add((int)cataloguePrice.CataloguePriceTierRef, cataloguePriceId);

                if (!string.IsNullOrEmpty(cataloguePrice.CataloguePriceIdRef))
                    catalogueItemIdPriceIdDictionary.Add(cataloguePrice.CataloguePriceIdRef, cataloguePriceId);
            }

            context[ScenarioContextKeys.CatalogueTierMapDictionary] = cataloguePriceDictionary;

            context[ScenarioContextKeys.CataloguePriceIdMapDictionary] = catalogueItemIdPriceIdDictionary;
        }

        [When(@"a GET request is made to retrieve the list of prices using catalogue item ID (.*)")]
        public async Task WhenAGetRequestIsMadeToRetrieveThePricingByCatalogueItemId(string catalogueItemId)
        {
            response.Result = await Client.GetAsync(
                string.Format(CultureInfo.InvariantCulture, getPricesByCatalogueItemIdUrlTemplate, catalogueItemId));
        }

        [When(@"a GET request is made to retrieve the list of prices")]
        public async Task WhenAGetRequestIsMadeToRetrieveTheListOfPrices()
        {
            response.Result = await Client.GetAsync(GetPricesUrl);
        }

        [When(@"a GET request is made to retrieve a single price using the PriceId associated with CataloguePriceIdRef (.*)")]
        public async Task WhenAGetRequestIsMadeToRetrieveThePriceUsingPriceIdAssociatedWithSolutionId(string cataloguePriceIdRef)
        {
            var cataloguePriceId = context.GetCataloguePriceIdsByCataloguePriceIdRef(cataloguePriceIdRef);
            response.Result = await Client.GetAsync(
                string.Format(CultureInfo.InvariantCulture, getPricesByPriceIdUrlTemplate, cataloguePriceId));
        }

        [Then(@"Prices are returned")]
        public async Task ThenPricesAreReturned(Table table)
        {
            var expected = table.CreateSet<PriceResultTable>().ToList();
            const string itemUnitToken = "itemUnit";
            const string timeUnitToken = "timeUnit";

            var content = (await response.ReadBody()).SelectToken(PriceToken)?.Select(t => new
            {
                Type = t.Value<string>("type"),
                ProvisioningType = t.Value<string>("provisioningType"),
                CurrencyCode = t.Value<string>("currencyCode"),
                Price = t.Value<decimal?>("price"),
                PricingItemName = t.SelectToken(itemUnitToken)?.Value<string>("name"),
                PricingItemDescription = t.SelectToken(itemUnitToken)?.Value<string>("description"),
                PricingItemTierName = t.SelectToken(itemUnitToken)?.Value<string>("tierName"),
                TimeUnitName = t.SelectToken(timeUnitToken)?.Value<string>("name"),
                TimeUnitDescription = t.SelectToken(timeUnitToken)?.Value<string>("description"),
            });

            content.Should().BeEquivalentTo(expected);
        }

        [Then(@"an empty price list is returned")]
        public async Task ThenAnEmptyListIsReturned()
        {
            var priceList = (await response.ReadBody()).SelectToken(PriceToken)?.ToList();
            priceList.Should().BeEmpty();
        }

        [Then(@"a Price is returned")]
        public async Task ThenPriceIsReturned(Table table)
        {
            var expected = table.CreateSet<PriceResultTable>().Single();
            const string itemUnitToken = "itemUnit";
            const string timeUnitToken = "timeUnit";

            var body = await response.ReadBody();
            var content = new
            {
                Type = body.Value<string>("type"),
                CurrencyCode = body.Value<string>("currencyCode"),
                Price = body.Value<decimal?>("price"),
                ProvisioningType = body.Value<string>("provisioningType"),
                PricingItemName = body.SelectToken(itemUnitToken)?.Value<string>("name"),
                PricingItemDescription = body.SelectToken(itemUnitToken)?.Value<string>("description"),
                PricingItemTierName = body.SelectToken(itemUnitToken)?.Value<string>("tierName"),
                TimeUnitName = body.SelectToken(timeUnitToken)?.Value<string>("name"),
                TimeUnitDescription = body.SelectToken(timeUnitToken)?.Value<string>("description"),
            };

            content.Should().BeEquivalentTo(expected);
        }

        [Then(@"the Prices Tiers are returned")]
        public async Task ThenThePricesTiersAreReturned(Table table)
        {
            var expectedTable = table.CreateSet<TierTable>().ToList();
            var listExpected = expectedTable.GroupBy(t => t.Section);

            var expected = listExpected.Select(g => g.Select(t => new
            {
                t.Start,
                t.End,
                t.Price,
            }));

            var pricesToken = (await response.ReadBody()).SelectToken(PriceToken);

            const string tierToken = "tiers";
            var tierPrices = pricesToken?.Where(t => t.SelectToken(tierToken) is not null);
            var content = tierPrices?.Select(t => new
            {
                Tier = t.SelectToken(tierToken)?.Select(jToken => new
                {
                    Start = jToken.Value<int?>("start"),
                    End = jToken.Value<int?>("end"),
                    Price = jToken.Value<decimal?>("price"),
                }),
            });

            content?.Select(p => p.Tier).Should().BeEquivalentTo(expected, c => c.WithoutStrictOrdering());
        }

        [Then(@"the Price Tiers are returned")]
        public async Task ThenThePriceTiersAreReturned(Table table)
        {
            var expectedTable = table.CreateSet<TierTable>().ToList();

            var expected = expectedTable.Select(t => new
            {
                t.Start,
                t.End,
                t.Price,
            });

            var pricesToken = await response.ReadBody();
            var tierPrices = pricesToken.SelectToken("tiers");

            var content = tierPrices?.Select(t => new
            {
                Start = t.Value<int?>("start"),
                End = t.Value<int?>("end"),
                Price = t.Value<decimal?>("price"),
            });

            content.Should().BeEquivalentTo(expected, c => c.WithoutStrictOrdering());
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class CataloguePriceTable
        {
            public string CatalogueItemId { get; init; }

            public CataloguePriceTypeEnum CataloguePriceTypeEnum { get; init; }

            public ProvisioningTypeEnum ProvisioningTypeEnum { get; init; }

            public string CurrencyCode { get; init; }

            public decimal? Price { get; init; }

            public Guid PricingUnitId { get; init; }

            public TimeUnit? TimeUnitEnum { get; init; }

            public int? CataloguePriceTierRef { get; init; }

            public string CataloguePriceIdRef { get; init; }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class PriceResultTable
        {
            public string Type { get; init; }

            public string CurrencyCode { get; init; }

            public decimal? Price { get; init; }

            public string PricingItemName { get; init; }

            public string PricingItemDescription { get; init; }

            public string PricingItemTierName { get; init; }

            public string TimeUnitName { get; init; }

            public string TimeUnitDescription { get; init; }

            public string ProvisioningType { get; init; }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class TierTable
        {
            public int? Start { get; init; }

            public int? End { get; init; }

            public decimal? Price { get; init; }

            public int? Section { get; init; }
        }
    }
}
