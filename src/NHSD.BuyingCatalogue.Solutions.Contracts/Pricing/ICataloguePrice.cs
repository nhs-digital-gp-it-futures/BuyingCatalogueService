namespace NHSD.BuyingCatalogue.Solutions.Contracts.Pricing
{
    public interface ICataloguePrice
    {
        int CataloguePriceId { get; }

        string CatalogueItemId { get; }
        string Type { get; }

        //IProvisioningType ProvisioningType { get; }

        string CataloguePriceType { get; }

        IPricingUnit PricingUnit { get; }

        ITimeUnit TimeUnit { get; }

        string CurrencyCode { get; }
    }
}
