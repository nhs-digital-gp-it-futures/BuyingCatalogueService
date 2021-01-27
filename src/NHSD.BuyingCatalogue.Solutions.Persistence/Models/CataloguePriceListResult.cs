using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    public sealed class CataloguePriceListResult : ICataloguePriceListResult
    {
        public int CataloguePriceId { get; init; }

        public string CatalogueItemName { get; init; }

        public string CatalogueItemId { get; init; }

        public int CataloguePriceTypeId { get; init; }

        public string PricingUnitName { get; init; }

        public string PricingUnitDescription { get; init; }

        public string PricingUnitTierName { get; init; }

        public int ProvisioningTypeId { get; init; }

        public int? TimeUnitId { get; init; }

        public string CurrencyCode { get; init; }

        public decimal? FlatPrice { get; init; }

        public int? BandStart { get; init; }

        public int? BandEnd { get; init; }

        public decimal? TieredPrice { get; init; }
    }
}
