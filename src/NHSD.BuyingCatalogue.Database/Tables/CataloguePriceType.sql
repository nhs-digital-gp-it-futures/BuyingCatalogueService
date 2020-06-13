CREATE TABLE dbo.CataloguePriceType
(
    CataloguePriceTypeId INT NOT NULL,
    [Name] NVARCHAR(10) NOT NULL
    CONSTRAINT PK_CataloguePriceType PRIMARY KEY (CataloguePriceTypeId)
)
