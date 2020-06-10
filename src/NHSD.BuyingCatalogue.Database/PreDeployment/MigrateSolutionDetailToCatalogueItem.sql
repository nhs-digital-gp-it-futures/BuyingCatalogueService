/*-----------------------------------------------------------------------
    Create migration schema
------------------------------------------------------------------------*/

IF NOT EXISTS (SELECT * FROM information_schema.schemata WHERE schema_name = 'migration')
    EXEC sp_executesql N'CREATE SCHEMA migration;'
GO

/*-----------------------------------------------------------------------
    Create migration tables
------------------------------------------------------------------------*/

IF OBJECT_ID('migration.CatalogueItem', 'U') IS NULL AND UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    CREATE TABLE migration.CatalogueItem
    (
        CatalogueItemId varchar(14) NOT NULL PRIMARY KEY,
        [Name] varchar(255) NULL,
        Created datetime2(7) NULL,
        CatalogueItemTypeId int NULL,
        SupplierId varchar(6) NULL,
        PublishedStatusId int NULL
    );
GO

IF OBJECT_ID('migration.Solution', 'U') IS NULL AND UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    CREATE TABLE migration.Solution
    (
         Id varchar(14) NOT NULL PRIMARY KEY,
         [Version] varchar(10) NULL,
         Features nvarchar(max) NULL,
         ClientApplication nvarchar(max) NULL,
         Hosting nvarchar(max) NULL,
         ImplementationDetail varchar(1000) NULL,
         RoadMap varchar(1000) NULL,
         IntegrationsUrl varchar(1000) NULL,
         AboutUrl varchar(1000) NULL,
         Summary varchar(300) NULL,
         FullDescription varchar(3000) NULL,
         LastUpdated datetime2(7) NOT NULL,
         LastUpdatedBy uniqueidentifier NOT NULL
    );
GO

IF OBJECT_ID('migration.FrameworkSolutions', 'U') IS NULL AND UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    CREATE TABLE migration.FrameworkSolutions
    (
         FrameworkId varchar(10) NOT NULL,
         SolutionId varchar(14) NOT NULL,
         IsFoundation bit NOT NULL,
         LastUpdated datetime2(7) NOT NULL,
         LastUpdatedBy uniqueidentifier NOT NULL
    );
GO

IF OBJECT_ID('migration.MarketingContact', 'U') IS NULL AND UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    CREATE TABLE migration.MarketingContact
    (
         Id int NOT NULL,
         SolutionId varchar(14) NOT NULL,
         FirstName varchar(35) NULL,
         LastName varchar(35) NULL,
         Email varchar(255) NULL,
         PhoneNumber varchar(35) NULL,
         Department varchar(50) NULL,
         LastUpdated datetime2(7) NOT NULL,
         LastUpdatedBy uniqueidentifier NOT NULL
    );
GO

IF OBJECT_ID('migration.SolutionCapability', 'U') IS NULL AND UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    CREATE TABLE migration.SolutionCapability
    (
         SolutionId varchar(14) NOT NULL,
         CapabilityId uniqueidentifier NOT NULL,
         StatusId int NOT NULL,
         LastUpdated datetime2(7) NOT NULL,
         LastUpdatedBy uniqueidentifier NOT NULL
    );
GO

IF OBJECT_ID('migration.SolutionEpic', 'U') IS NULL AND UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    CREATE TABLE migration.SolutionEpic
    (
         SolutionId varchar(14) NOT NULL,
         CapabilityId uniqueidentifier NOT NULL,
         EpicId varchar(10) NOT NULL,
         StatusId int NOT NULL,
         LastUpdated datetime2(7) NOT NULL,
         LastUpdatedBy uniqueidentifier NOT NULL
    );
GO

/*-----------------------------------------------------------------------
    Truncate migration tables (in case they existed previously)
------------------------------------------------------------------------*/

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE' AND EXISTS (SELECT * FROM migration.CatalogueItem)
    TRUNCATE TABLE migration.CatalogueItem;
GO

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE' AND EXISTS (SELECT * FROM migration.Solution)
    TRUNCATE TABLE migration.Solution;
GO

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE' AND EXISTS (SELECT * FROM migration.FrameworkSolutions)
    TRUNCATE TABLE migration.FrameworkSolutions;
GO

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE' AND EXISTS (SELECT * FROM migration.MarketingContact)
    TRUNCATE TABLE migration.MarketingContact;
GO

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE' AND EXISTS (SELECT * FROM migration.SolutionCapability)
    TRUNCATE TABLE migration.SolutionCapability;
GO

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE' AND EXISTS (SELECT * FROM migration.SolutionEpic)
    TRUNCATE TABLE migration.SolutionEpic;
GO

/*-----------------------------------------------------------------------
    Copy data to migration tables
------------------------------------------------------------------------*/


DECLARE @solutionCatalogueItemType int = 1;

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    INSERT INTO migration.CatalogueItem(CatalogueItemId, [Name], Created, CatalogueItemTypeId, SupplierId, PublishedStatusId)
         SELECT Id, [Name], LastUpdated, @solutionCatalogueItemType, SupplierId, PublishedStatusId
           FROM dbo.Solution;
GO

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    INSERT INTO migration.Solution(Id, [Version],
                Features, ClientApplication, Hosting, ImplementationDetail, RoadMap, IntegrationsUrl, AboutUrl, Summary, FullDescription,
                LastUpdated, LastUpdatedBy)
         SELECT s.Id, s.[Version],
                d.Features, d.ClientApplication, d.Hosting, d.ImplementationDetail, d.RoadMap, d.IntegrationsUrl, d.AboutUrl, d.Summary, d.FullDescription,
                s.LastUpdated, s.LastUpdatedBy
           FROM dbo.Solution AS s
                INNER JOIN dbo.SolutionDetail AS d
                ON s.Id = d.SolutionId;
GO

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    INSERT INTO migration.FrameworkSolutions(FrameworkId, SolutionId, IsFoundation, LastUpdated, LastUpdatedBy)
         SELECT FrameworkId, SolutionId, IsFoundation, LastUpdated, LastUpdatedBy
           FROM dbo.FrameworkSolutions;
GO

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    INSERT INTO migration.MarketingContact(Id, SolutionId, FirstName, LastName, Email, PhoneNumber, Department, LastUpdated, LastUpdatedBy)
         SELECT Id, SolutionId, FirstName, LastName, Email, PhoneNumber, Department, LastUpdated, LastUpdatedBy
           FROM dbo.MarketingContact;
GO

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    INSERT INTO migration.SolutionCapability(SolutionId, CapabilityId, StatusId, LastUpdated, LastUpdatedBy)
         SELECT SolutionId, CapabilityId, StatusId, LastUpdated, LastUpdatedBy
           FROM dbo.SolutionCapability;
GO

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    INSERT INTO migration.SolutionEpic(SolutionId, CapabilityId, EpicId, StatusId, LastUpdated, LastUpdatedBy)
         SELECT SolutionId, CapabilityId, EpicId, StatusId, LastUpdated, LastUpdatedBy
           FROM dbo.SolutionEpic;
GO

/*-----------------------------------------------------------------------
    Delete existing solution data
------------------------------------------------------------------------*/

DELETE FROM dbo.Solution;
GO
