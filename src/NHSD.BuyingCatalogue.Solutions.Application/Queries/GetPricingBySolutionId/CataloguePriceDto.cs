using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId
{
    public abstract class CataloguePriceDto : ICataloguePrice
    {
        public int CataloguePriceId { get; set; }

        public string CatalogueItemId { get; set; }

        public string CurrencyCode { get; set; }
    }
}
