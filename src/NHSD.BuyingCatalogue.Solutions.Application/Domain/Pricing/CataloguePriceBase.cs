namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing
{
    public abstract class CataloguePriceBase
    {
        public int CataloguePriceId { get; set; }

        public string CatalogueItemName { get; set; }

        public string CatalogueItemId { get; set; }

        public CataloguePriceType CataloguePriceType { get; }

        public ProvisioningType ProvisioningType { get; set; }

        public PricingUnit PricingUnit { get; set; }

        public TimeUnit TimeUnit { get; set; }

        public string CurrencyCode { get; set; }

        protected CataloguePriceBase(CataloguePriceType type)
        {
            CataloguePriceType = type;
        }
    }
}
