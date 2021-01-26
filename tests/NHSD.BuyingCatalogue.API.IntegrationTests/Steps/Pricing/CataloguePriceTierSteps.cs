using System.Threading.Tasks;
using JetBrains.Annotations;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Pricing
{
    [Binding]
    internal sealed class CataloguePriceTierSteps
    {
        private readonly ScenarioContext context;

        public CataloguePriceTierSteps(ScenarioContext context)
        {
            this.context = context;
        }

        [Given(@"CataloguePriceTier exists")]
        public async Task GivenCataloguePriceTierExists(Table table)
        {
            foreach (var tierTable in table.CreateSet<CataloguePriceTierTable>())
            {
                int cataloguePriceId =
                    context.GetCataloguePriceIdByCataloguePriceTierReference(tierTable.CataloguePriceTierRef);

                await CataloguePriceTierEntityBuilder.Create()
                    .WithCataloguePriceId(cataloguePriceId)
                    .WithBandStart(tierTable.BandStart)
                    .WithBandEnd(tierTable.BandEnd)
                    .WithPrice(tierTable.Price)
                    .Build()
                    .InsertAsync();
            }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class CataloguePriceTierTable
        {
            public int CataloguePriceTierRef { get; init; }

            public int BandStart { get; init; }

            public int? BandEnd { get; init; }

            public decimal Price { get; init; }
        }
    }
}
