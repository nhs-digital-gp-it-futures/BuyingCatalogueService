using System;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class CataloguePriceEntity : EntityBase
    {
        public string CatalogueItemId { get; set; }
        public int ProvisioningTypeId { get; set; }
        public int CataloguePriceTypeId { get; set; }
        public Guid PricingUnitId { get; set; }
        public int? TimeUnitId { get; set; }
        public string CurrencyCode { get; set; }
        public decimal? Price { get; set; }

        protected override string InsertSql => @"
        INSERT INTO dbo.CataloguePrice
        (
            CatalogueItemId,
            ProvisioningTypeId,
            CataloguePriceTypeId,
            PricingUnitId,
            TimeUnitId,
            CurrencyCode,
            Price,
            LastUpdated
        )
        VALUES
        (
            @CatalogueItemId,
            @ProvisioningTypeId,
            @CataloguePriceTypeId,
            @PricingUnitId,
            @TimeUnitId,
            @CurrencyCode,
            @Price,
            @LastUpdated
        );";
    }
}
