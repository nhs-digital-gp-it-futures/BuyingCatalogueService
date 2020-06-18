CREATE PROCEDURE test.ClearData AS
    SET NOCOUNT ON;

    TRUNCATE TABLE dbo.FrameworkCapabilities;
    
    DELETE FROM dbo.CataloguePriceTier;
    DELETE FROM dbo.CataloguePrice;
    DELETE FROM dbo.CatalogueItem;
    DELETE FROM dbo.Epic;
    DELETE FROM dbo.Capability;
    DELETE FROM dbo.Supplier;

    ALTER ROLE Api
    ADD MEMBER [NHSD-BAPI];
GO
