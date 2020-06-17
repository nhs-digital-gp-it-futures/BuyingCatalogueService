namespace NHSD.BuyingCatalogue.Solutions.Contracts.Pricing
{
    public interface ICataloguePrice
    {
        int CataloguePriceId { get; }

        string CatalogueItemId { get; }

        //IProvisioningType ProvisioningType { get; }

        //ICataloguePriceType CataloguePriceType { get; }

        //IPricingUnit PricingUnit { get; }

        //ITimeUnit TimeUnit { get; }

        string CurrencyCode { get; }
    }
}
