/*-----------------------------------------------------------------------
    Copy data from migration tables
------------------------------------------------------------------------*/
DECLARE @solutionCatalogueItemType int = 1;

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    INSERT INTO dbo.CatalogueItem(CatalogueItemId, [Name], Created, CatalogueItemTypeId, SupplierId, PublishedStatusId)
         SELECT CatalogueItemId, [Name], Created, @solutionCatalogueItemType, SupplierId, PublishedStatusId
           FROM migration.CatalogueItem;
GO

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    INSERT INTO dbo.Solution(Id, [Version], Summary, FullDescription, Features, ClientApplication, Hosting,
                ImplementationDetail, RoadMap, IntegrationsUrl, AboutUrl, LastUpdated, LastUpdatedBy)
         SELECT Id, [Version], Summary, FullDescription, Features, ClientApplication, Hosting,
                ImplementationDetail, RoadMap, IntegrationsUrl, AboutUrl, LastUpdated, LastUpdatedBy
           FROM migration.Solution;
GO

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    INSERT INTO dbo.FrameworkSolutions(FrameworkId, SolutionId, IsFoundation, LastUpdated, LastUpdatedBy)
         SELECT FrameworkId, SolutionId, IsFoundation, LastUpdated, LastUpdatedBy
           FROM migration.FrameworkSolutions;
GO

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
BEGIN
    SET IDENTITY_INSERT dbo.MarketingContact ON;

    INSERT INTO dbo.MarketingContact(Id, SolutionId, FirstName, LastName, Email, PhoneNumber, Department, LastUpdated, LastUpdatedBy)
         SELECT Id, SolutionId, FirstName, LastName, Email, PhoneNumber, Department, LastUpdated, LastUpdatedBy
           FROM migration.MarketingContact;

    SET IDENTITY_INSERT dbo.MarketingContact OFF;
END;
GO

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    INSERT INTO dbo.SolutionCapability(SolutionId, CapabilityId, StatusId, LastUpdated, LastUpdatedBy)
         SELECT SolutionId, CapabilityId, StatusId, LastUpdated, LastUpdatedBy
           FROM migration.SolutionCapability;
GO

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    INSERT INTO dbo.SolutionEpic(SolutionId, CapabilityId, EpicId, StatusId, LastUpdated, LastUpdatedBy)
         SELECT SolutionId, CapabilityId, EpicId, StatusId, LastUpdated, LastUpdatedBy
           FROM migration.SolutionEpic;
GO

/*-----------------------------------------------------------------------
    Drop unrequired tables
------------------------------------------------------------------------*/

DROP TABLE IF EXISTS
    dbo.SolutionDetail,
    dbo.SolutionAuthorityStatus,
    dbo.SolutionSupplierStatus,
    AdditionalServicePrice,
    AssociatedServicePrice,
    SolutionPrice,
    PurchasingModel,
    PriceType,
    AdditionalServiceDetail,
    [Audit],
    SolutionStandard,
    SolutionStandardStatus,
    FrameworkStandards,
    [Standard],
    StandardStatus,
    StandardCatery,
    CapabilityStandards,
    SolutionDefinedEpicAcceptanceCriteria,
    SolutionDefinedEpic,
    SolutionDefinedCapability,
    Organisation;
GO

/*-----------------------------------------------------------------------
    Drop migration tables and schema
------------------------------------------------------------------------*/

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    DROP TABLE IF EXISTS
         migration.CatalogueItem,
         migration.Solution,
         migration.FrameworkSolutions,
         migration.MarketingContact,
         migration.SolutionCapability,
         migration.SolutionEpic;
GO

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    DROP SCHEMA IF EXISTS migration;
GO
