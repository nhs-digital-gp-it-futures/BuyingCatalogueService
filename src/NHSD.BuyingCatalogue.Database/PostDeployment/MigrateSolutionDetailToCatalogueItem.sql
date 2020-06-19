CREATE OR ALTER PROCEDURE migration.PostDeployment
AS
    SET NOCOUNT ON;

    IF EXISTS (SELECT * FROM dbo.CatalogueItem)
        RETURN;

    /*-----------------------------------------------------------------------
        Copy data from migration tables
    ------------------------------------------------------------------------*/
    DECLARE @solutionCatalogueItemType int = 1;

    INSERT INTO dbo.CatalogueItem(CatalogueItemId, [Name], Created, CatalogueItemTypeId, SupplierId, PublishedStatusId)
         SELECT CatalogueItemId, [Name], Created, @solutionCatalogueItemType, SupplierId, PublishedStatusId
           FROM migration.CatalogueItem;
    
    INSERT INTO dbo.Solution(Id, [Version], Summary, FullDescription, Features, ClientApplication, Hosting,
                ImplementationDetail, RoadMap, IntegrationsUrl, AboutUrl, LastUpdated, LastUpdatedBy)
         SELECT Id, [Version], Summary, FullDescription, Features, ClientApplication, Hosting,
                ImplementationDetail, RoadMap, IntegrationsUrl, AboutUrl, LastUpdated, LastUpdatedBy
           FROM migration.NewSolution;
    
    INSERT INTO dbo.FrameworkSolutions(FrameworkId, SolutionId, IsFoundation, LastUpdated, LastUpdatedBy)
         SELECT FrameworkId, SolutionId, IsFoundation, LastUpdated, LastUpdatedBy
           FROM migration.FrameworkSolutions;
    
    SET IDENTITY_INSERT dbo.MarketingContact ON;

    INSERT INTO dbo.MarketingContact(Id, SolutionId, FirstName, LastName, Email, PhoneNumber, Department, LastUpdated, LastUpdatedBy)
         SELECT Id, SolutionId, FirstName, LastName, Email, PhoneNumber, Department, LastUpdated, LastUpdatedBy
           FROM migration.MarketingContact;

    SET IDENTITY_INSERT dbo.MarketingContact OFF;
    
    INSERT INTO dbo.SolutionCapability(SolutionId, CapabilityId, StatusId, LastUpdated, LastUpdatedBy)
         SELECT SolutionId, CapabilityId, StatusId, LastUpdated, LastUpdatedBy
           FROM migration.SolutionCapability;
    
    INSERT INTO dbo.SolutionEpic(SolutionId, CapabilityId, EpicId, StatusId, LastUpdated, LastUpdatedBy)
         SELECT SolutionId, CapabilityId, EpicId, StatusId, LastUpdated, LastUpdatedBy
           FROM migration.SolutionEpic;

    /*-----------------------------------------------------------------------
        Drop unrequired tables
    ------------------------------------------------------------------------*/

    DROP TABLE IF EXISTS
        dbo.SolutionDetail,
        dbo.SolutionSupplierStatus,
        dbo.AdditionalServicePrice,
        dbo.AssociatedServicePrice,
        dbo.SolutionPrice,
        dbo.PurchasingModel,
        dbo.PriceType,
        dbo.AdditionalServiceDetail,
        dbo.[Audit],
        dbo.CapabilityStandards,
        dbo.SolutionStandard,
        dbo.SolutionStandardStatus,
        dbo.FrameworkStandards,
        dbo.[Standard],
        dbo.StandardStatus,
        dbo.StandardCategory,
        dbo.SolutionDefinedEpicAcceptanceCriteria,
        dbo.SolutionDefinedEpic,
        dbo.SolutionDefinedCapability,
        dbo.SolutionAuthorityStatus,
        dbo.Organisation;

    /*-----------------------------------------------------------------------
        Drop unrequired stored procedures
    ------------------------------------------------------------------------*/

    DROP PROCEDURE IF EXISTS dbo.PublishSolution, dbo.SolutionImport;

    /*-----------------------------------------------------------------------
        Drop unrequired types
    ------------------------------------------------------------------------*/

    DROP TYPE IF EXISTS dbo.SolutionImportCapability;
    DROP TYPE IF EXISTS import.AdditionalServiceCapability;

    /*-----------------------------------------------------------------------
        Drop migration tables and schema
    ------------------------------------------------------------------------*/

    DROP TABLE IF EXISTS
         migration.CatalogueItem,
         migration.OldSolution,
         migration.NewSolution,
         migration.FrameworkSolutions,
         migration.MarketingContact,
         migration.SolutionCapability,
         migration.SolutionEpic;
GO
