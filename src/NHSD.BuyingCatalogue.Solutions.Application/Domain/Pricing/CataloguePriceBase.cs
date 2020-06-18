﻿namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing
{
    public abstract class CataloguePriceBase
    {
        public int CataloguePriceId { get; set; }

        public string CatalogueItemId { get; set; }

        public string Type { get; set; }

        public ProvisioningType ProvisioningType { get; set; }

        public CataloguePriceType CataloguePriceType { get; }

        public PricingUnit PricingUnit { get; set; }

        public TimeUnit TimeUnit { get; set; }

        public string CurrencyCode { get; set; }

        protected CataloguePriceBase(CataloguePriceType type)
        {
            CataloguePriceType = type;
        }
    }
}
