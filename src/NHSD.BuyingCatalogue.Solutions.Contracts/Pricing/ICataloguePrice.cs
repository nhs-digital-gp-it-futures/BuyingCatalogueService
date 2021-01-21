namespace NHSD.BuyingCatalogue.Solutions.Contracts.Pricing
{
    public interface ICataloguePrice
    {
        int CataloguePriceId { get; }

        string CatalogueItemName { get; }

        string CatalogueItemId { get; }

        string Type { get; }

        string ProvisioningType { get; }

        IPricingUnit PricingUnit { get; }

        ITimeUnit TimeUnit { get; }

        string CurrencyCode { get; }
    }
}
