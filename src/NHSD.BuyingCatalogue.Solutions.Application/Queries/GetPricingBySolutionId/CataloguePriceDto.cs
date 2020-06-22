using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId
{
    public abstract class CataloguePriceDto : ICataloguePrice
    {
        public int CataloguePriceId { get; set; }
        public string CatalogueItemName { get; set; }
        public string CatalogueItemId { get; set; }
        public string Type { get; set; }
        public string CataloguePriceType { get; }

        public IPricingUnit PricingUnit { get; set; }

        public ITimeUnit TimeUnit { get; set; }

        //public IProvisioningType ProvisioningType { get; set; }

        public string CurrencyCode { get; set; }
    }
}
