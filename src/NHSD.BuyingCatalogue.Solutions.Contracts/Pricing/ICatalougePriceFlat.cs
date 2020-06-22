using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Pricing
{
    interface ICatalougePriceFlat
    {
        ICataloguePriceListResult CataloguePrice { get; set; }

        decimal Price { get; set; }
    }
}
