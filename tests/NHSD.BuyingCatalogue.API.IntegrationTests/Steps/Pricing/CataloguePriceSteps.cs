﻿using System;
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

        private const string pricingUrl = "http://localhost:5200/api/v1/solutions/{0}/pricing";
        private readonly string priceToken = "prices";

        public CataloguePriceSteps(Response response)
        {
            _response = response;
        }

        [Given(@"CataloguePrice exists")]
        public static async Task GivenCataloguePriceExists(Table table)
        {
            foreach (var cataloguePrice in table.CreateSet<CataloguePriceTable>())
            {
                await CataloguePriceEntityBuilder.Create()
                    .WithCatalogueItemId(cataloguePrice.CatalogueItemId)
                    .WithCurrencyCode(cataloguePrice.CurrencyCode)
                    .WithPrice(cataloguePrice.Price)
                    .WithPricingUnitId(cataloguePrice.PricingUnitId)
                    .WithTimeUnit(cataloguePrice.TimeUnitId)
                    .Build()
                    .InsertAsync();
            }
        }

        [When(@"a GET request is made to retrieve the pricing with Solution ID (.*)")]
        public async Task WhenAGETRequestIsMadeToRetrieveThePricingWithSolutionID(string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(CultureInfo.InvariantCulture, pricingUrl, solutionId));
        }

        [Then(@"Prices are returned")]
        public async Task ThenPricesAreReturned(Table table)
        {
            var expected = table.CreateSet<PriceResultTable>().ToList();

            var content = (await _response.ReadBody()).SelectToken(priceToken).Select(x => new PriceResultTable
            {
                Type = x.Value<string>("type"),
                CurrencyCode = x.Value<string>("currencyCode"),
                Price = x.Value<decimal?>("price")
            });

            content.Should().BeEquivalentTo(expected);
        }

        //NEEDS FIXING
        [Then(@"has Pricing Item Unit")]
        public async Task ThenHasPricingItemUnit(Table table)
        {
            var expected = table.CreateSet<ItemUnitTable>().ToList();

            var a = (await _response.ReadBody()).SelectToken(priceToken).SelectToken("itemUnit");

            var content = (await _response.ReadBody()).SelectToken(priceToken).SelectToken("itemUnit").Select(x => new ItemUnitTable
            {
                Name = x.Value<string>("name"),
                Description = x.Value<string>("description"),
                TierName = x.Value<string>("tierName")
            });

            content.Should().BeEquivalentTo(expected);
        }

        [Then(@"has Pricing Time Unit")]
        public async Task ThenHasPricingTimeUnit(Table table)
        {
            var expected = table.CreateSet<TimeUnitTable>().ToList();

            var content = (await _response.ReadBody()).SelectToken(priceToken).SelectToken("timeUnit").Select(x => new TimeUnitTable
            {
                Name = x.Value<string>("name"),
                Description = x.Value<string>("description")
            });

            content.Should().BeEquivalentTo(expected);
        }


        public sealed class CataloguePriceTable
        {
            public string CatalogueItemId { get; set; }
            public string CurrencyCode { get; set; }
            public decimal? Price { get; set; }
            public Guid PricingUnitId { get; set; }
            public int TimeUnitId { get; set; }
        }

        public sealed class PriceResultTable
        {
            public string Type { get; set; }
            public string CurrencyCode { get; set; }
            public decimal? Price { get; set; }
        }

        public sealed class ItemUnitTable
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string TierName { get; set; }
        }

        public sealed class TimeUnitTable
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
