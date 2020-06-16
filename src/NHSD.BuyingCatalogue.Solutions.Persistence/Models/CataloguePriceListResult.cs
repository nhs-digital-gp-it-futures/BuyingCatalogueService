using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    public sealed class CataloguePriceListResult : ICataloguePriceListResult
    {
        public int CataloguePriceId { get; set; }
        public string CatalogueItemId { get; set; }
        public int ProvisioningTypeId { get; set; }
        public int CataloguePriceTypeId { get; set; }
        public IPricingUnit PricingUnit { get; set; }
        public int TimeUnitId { get; set; }
        public string CurrencyCode { get; set; }
        public decimal? FlatPrice { get; set; }
        public int? BandStart { get; set; }
        public int? BandEnd { get; set; }
        public decimal? TieredPrice { get; set; }
    }
}
