using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ICataloguePriceListResult
    {
        int CataloguePriceId { get; }

        string CatalogueItemId { get; }

        int ProvisioningTypeId { get; }

        int CataloguePriceTypeId { get; }

        string PricingUnitName { get; }
        string PricingUnitDescription { get; }
        string PricingUnitTierName { get; }

        //IProvisioningType ProvisioningType { get;}

        //ICataloguePriceType CataloguePriceType { get; }

        //IPricingUnit PricingUnit { get; }

        int TimeUnitId { get; }

        //ITimeUnit TimeUnit { get; }

        string CurrencyCode { get; }

        decimal? FlatPrice { get; }

        int? BandStart { get; }

        int? BandEnd { get; }

        decimal? TieredPrice { get; }
    }
}
