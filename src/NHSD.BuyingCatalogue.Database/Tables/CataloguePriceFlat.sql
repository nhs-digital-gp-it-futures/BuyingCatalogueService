CREATE TABLE dbo.CataloguePriceFlat
(
    CataloguePriceId INT NOT NULL,
    Price DECIMAL(18,3) NOT NULL
    CONSTRAINT PK_CataloguePriceFlat PRIMARY KEY NONCLUSTERED (CataloguePriceId),
    CONSTRAINT FK_CataloguePriceFlat_CataloguePrice_CataloguePriceId FOREIGN KEY (CataloguePriceId) REFERENCES dbo.CataloguePrice(CataloguePriceId)
)
