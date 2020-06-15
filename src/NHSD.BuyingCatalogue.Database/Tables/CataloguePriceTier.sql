CREATE TABLE dbo.CataloguePriceTier
(
    CataloguePriceTierId INT IDENTITY(1,1) NOT NULL,
    CataloguePriceId INT NOT NULL,
    BandStart INT NOT NULL,
    BandEnd INT NULL,
    Price DECIMAL(18,3) NOT NULL,
    CONSTRAINT PK_CataloguePriceTier PRIMARY KEY NONCLUSTERED (CataloguePriceTierId),
    CONSTRAINT FK_CataloguePriceTier_CataloguePrice_CataloguePriceId FOREIGN KEY (CataloguePriceId) REFERENCES dbo.CataloguePrice(CataloguePriceId)
)
