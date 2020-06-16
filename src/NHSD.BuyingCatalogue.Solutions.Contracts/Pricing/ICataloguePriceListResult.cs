using System;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Pricing
{
    public interface ICataloguePriceListResult
    {
        int CataloguePriceId { get; }

        string CatalogueItemId { get; }

        int ProvisioningTypeId { get; }

        int CataloguePriceTypeId { get; }

        //IProvisioningType ProvisioningType { get;}

        //ICataloguePriceType CataloguePriceType { get; }

        IPricingUnit PricingUnit { get; }

        int TimeUnitId { get; }

        //ITimeUnit TimeUnit { get; }

        string CurrencyCode { get; }

        decimal? FlatPrice { get; }

        int? BandStart { get; }

        int? BandEnd { get; }

        decimal? TieredPrice { get; }
    }
}
