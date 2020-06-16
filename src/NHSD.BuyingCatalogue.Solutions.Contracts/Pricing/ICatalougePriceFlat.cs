namespace NHSD.BuyingCatalogue.Solutions.Contracts.Pricing
{
    interface ICatalougePriceFlat
    {
        ICataloguePriceListResult CataloguePrice { get; set; }

        decimal Price { get; set; }
    }
}
