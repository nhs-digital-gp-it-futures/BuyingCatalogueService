using System.Globalization;
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
            var expected = table.CreateSet<PriceResultTable>();

            var content = await _response.ReadBody();
            content.Should().BeEquivalentTo(expected);
        }


        public class CataloguePriceTable
        {
            public string CatalogueItemId { get; set; }
            public string CurrencyCode { get; set; }
        }

        public class PriceResultTable
        {
            public int PriceId { get; set; }
            public string CurrencyCode { get; set; }
        }
    }
}
