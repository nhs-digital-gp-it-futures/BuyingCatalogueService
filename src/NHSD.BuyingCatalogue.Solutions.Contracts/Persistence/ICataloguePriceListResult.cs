namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ICataloguePriceListResult
    {
        int CataloguePriceId { get; }

        string CatalogueItemName { get; }

        string CatalogueItemId { get; }

        int CataloguePriceTypeId { get; }

        string PricingUnitName { get; }

        string PricingUnitDescription { get; }

        string PricingUnitTierName { get; }

        int ProvisioningTypeId { get; }

        int? TimeUnitId { get; }

        string CurrencyCode { get; }

        decimal? FlatPrice { get; }

        int? BandStart { get; }

        int? BandEnd { get; }

        decimal? TieredPrice { get; }
    }
}
