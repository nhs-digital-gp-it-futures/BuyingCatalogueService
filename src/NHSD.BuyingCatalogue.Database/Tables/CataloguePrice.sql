CREATE TABLE dbo.CataloguePrice
(
    CataloguePriceId INT IDENTITY(1,1) NOT NULL,
    CatalogueItemId NVARCHAR(14),
    ProvisioningTypeId INT NOT NULL,
    CataloguePriceTypeId INT NOT NULL,
    PricingUnitId INT NOT NULL,
    TimeUnitId INT NULL,
    CurrencyCode NVARCHAR(3) NOT NULL,
    LastUpdated DATETIME2(7) NOT NULL

    CONSTRAINT PK_CataloguePrice PRIMARY KEY NONCLUSTERED (CatalogueItemId),
    CONSTRAINT FK_CataloguePrice_ProvisioningType_ProvisioningTypeId FOREIGN KEY (ProvisioningTypeId) REFERENCES dbo.ProvisioningType(ProvisioningTypeId),
    CONSTRAINT FK_CataloguePrice_CataloguePriceType_CataloguePriceTypeId FOREIGN KEY (CataloguePriceTypeId) REFERENCES dbo.CataloguePriceType(CataloguePriceTypeId),
    CONSTRAINT FK_CataloguePrice_TimeUnit_TimeUnitId FOREIGN KEY (TimeUnitId) REFERENCES dbo.TimeUnit(TimeUnitId),
    CONSTRAINT FK_CataloguePrice_PricingUnit_PricingUnitId FOREIGN KEY (PricingUnitId) REFERENCES dbo.PricingUnit(PricingUnitId)
)
