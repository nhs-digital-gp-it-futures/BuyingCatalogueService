using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Pricing
{
    [Binding]
    internal sealed class CatalougePriceTierSteps
    {
        private readonly ScenarioContext _context;

        public CatalougePriceTierSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Given(@"CataloguePriceTier exists")]
        public async Task GivenCataloguePriceTierExists(Table table)
        {
            foreach (var tierTable in table.CreateSet<CataloguePriceTierTable>())
            {
                int cataloguePriceId =
                    _context.GetCataloguePriceIdByCatalougePriceTierReference(tierTable.CatalougePriceTierRef);

                await CataloguePriceTierEntityBuilder.Create()
                    .WithCataloguePriceId(cataloguePriceId)
                    .WithBandStart(tierTable.BandStart)
                    .WithBandEnd(tierTable.BandEnd)
                    .WithPrice(tierTable.Price)
                    .Build()
                    .InsertAsync();
            }
        }

        public sealed class CataloguePriceTierTable
        {
            public int CataloguePriceTierRef { get; set; }
            public int BandStart { get; set; }
            public int? BandEnd { get; set; }
            public decimal Price { get; set; }
        }
    }
}
