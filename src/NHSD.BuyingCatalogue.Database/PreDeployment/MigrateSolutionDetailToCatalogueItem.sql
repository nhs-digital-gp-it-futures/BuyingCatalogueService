CREATE OR ALTER PROCEDURE migration.PreDeployment
AS
    SET NOCOUNT ON;

    IF OBJECT_ID(N'dbo.CatalogueItem', N'U') IS NOT NULL
        RETURN;

    /*-----------------------------------------------------------------------
        Create migration tables
    ------------------------------------------------------------------------*/

    IF OBJECT_ID('migration.CatalogueItem', 'U') IS NULL
        CREATE TABLE migration.CatalogueItem
        (
            CatalogueItemId varchar(14) NOT NULL PRIMARY KEY,
            [Name] varchar(255) NULL,
            Created datetime2(7) NULL,
            CatalogueItemTypeId int NULL,
            SupplierId varchar(6) NULL,
            PublishedStatusId int NULL
        );

    IF OBJECT_ID('migration.OldSolution', 'U') IS NULL
        CREATE TABLE migration.OldSolution
        (
            Id varchar(14) PRIMARY KEY NOT NULL,
            ParentId varchar(14) NULL,
            SupplierId varchar(6) NOT NULL,
            SolutionDetailId uniqueidentifier NULL,
            [Name] varchar(255) NOT NULL,
            [Version] varchar(10) NULL,
            PublishedStatusId int NOT NULL,
            AuthorityStatusId int NOT NULL,
            SupplierStatusId int NOT NULL,
            OnCatalogueVersion int NOT NULL,
            ServiceLevelAgreement nvarchar(1000) NULL,
            WorkOfPlan nvarchar(max) NULL,
            LastUpdated datetime2(7) NOT NULL,
            LastUpdatedBy uniqueidentifier NOT NULL
        );

    IF OBJECT_ID('migration.NewSolution', 'U') IS NULL
        CREATE TABLE migration.NewSolution
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

    IF OBJECT_ID('migration.FrameworkSolutions', 'U') IS NULL
        CREATE TABLE migration.FrameworkSolutions
        (
             FrameworkId varchar(10) NOT NULL,
             SolutionId varchar(14) NOT NULL,
             IsFoundation bit NOT NULL,
             LastUpdated datetime2(7) NOT NULL,
             LastUpdatedBy uniqueidentifier NOT NULL
        );

    IF OBJECT_ID('migration.MarketingContact', 'U') IS NULL
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

    IF OBJECT_ID('migration.SolutionCapability', 'U') IS NULL
        CREATE TABLE migration.SolutionCapability
        (
             SolutionId varchar(14) NOT NULL,
             CapabilityId uniqueidentifier NOT NULL,
             StatusId int NOT NULL,
             LastUpdated datetime2(7) NOT NULL,
             LastUpdatedBy uniqueidentifier NOT NULL
        );

    IF OBJECT_ID('migration.SolutionEpic', 'U') IS NULL
        CREATE TABLE migration.SolutionEpic
        (
             SolutionId varchar(14) NOT NULL,
             CapabilityId uniqueidentifier NOT NULL,
             EpicId varchar(10) NOT NULL,
             StatusId int NOT NULL,
             LastUpdated datetime2(7) NOT NULL,
             LastUpdatedBy uniqueidentifier NOT NULL
        );

    /*-----------------------------------------------------------------------
        Truncate migration tables (in case they existed previously)
    ------------------------------------------------------------------------*/

    IF EXISTS (SELECT * FROM migration.CatalogueItem)
        TRUNCATE TABLE migration.CatalogueItem;

    IF EXISTS (SELECT * FROM migration.OldSolution)
        TRUNCATE TABLE migration.OldSolution;

    IF EXISTS (SELECT * FROM migration.NewSolution)
        TRUNCATE TABLE migration.NewSolution;

    IF EXISTS (SELECT * FROM migration.FrameworkSolutions)
        TRUNCATE TABLE migration.FrameworkSolutions;

    IF EXISTS (SELECT * FROM migration.MarketingContact)
        TRUNCATE TABLE migration.MarketingContact;

    IF EXISTS (SELECT * FROM migration.SolutionCapability)
        TRUNCATE TABLE migration.SolutionCapability;

    IF EXISTS (SELECT * FROM migration.SolutionEpic)
        TRUNCATE TABLE migration.SolutionEpic;

    /*-----------------------------------------------------------------------
        Copy data to migration tables
    ------------------------------------------------------------------------*/

    DECLARE @solutionCatalogueItemType int = 1;

    -- The SELECT * is deliberate to avoid name resolution errors on redeployments
    -- where migration is not taking place
    INSERT INTO migration.OldSolution
         SELECT *
           FROM dbo.Solution;

    INSERT INTO migration.CatalogueItem(CatalogueItemId, [Name], Created, CatalogueItemTypeId, SupplierId, PublishedStatusId)
         SELECT Id, [Name], LastUpdated, @solutionCatalogueItemType, SupplierId, PublishedStatusId
           FROM migration.OldSolution;

    INSERT INTO migration.NewSolution(Id, [Version],
                Features, ClientApplication, Hosting, ImplementationDetail, RoadMap, IntegrationsUrl, AboutUrl, Summary, FullDescription,
                LastUpdated, LastUpdatedBy)
         SELECT s.Id, s.[Version],
                d.Features, d.ClientApplication, d.Hosting, d.ImplementationDetail, d.RoadMap, d.IntegrationsUrl, d.AboutUrl, d.Summary, d.FullDescription,
                s.LastUpdated, s.LastUpdatedBy
           FROM dbo.Solution AS s
                INNER JOIN dbo.SolutionDetail AS d
                ON s.Id = d.SolutionId;

    INSERT INTO migration.FrameworkSolutions(FrameworkId, SolutionId, IsFoundation, LastUpdated, LastUpdatedBy)
         SELECT FrameworkId, SolutionId, IsFoundation, LastUpdated, LastUpdatedBy
           FROM dbo.FrameworkSolutions;

    INSERT INTO migration.MarketingContact(Id, SolutionId, FirstName, LastName, Email, PhoneNumber, Department, LastUpdated, LastUpdatedBy)
         SELECT Id, SolutionId, FirstName, LastName, Email, PhoneNumber, Department, LastUpdated, LastUpdatedBy
           FROM dbo.MarketingContact;

    INSERT INTO migration.SolutionCapability(SolutionId, CapabilityId, StatusId, LastUpdated, LastUpdatedBy)
         SELECT SolutionId, CapabilityId, StatusId, LastUpdated, LastUpdatedBy
           FROM dbo.SolutionCapability;

    INSERT INTO migration.SolutionEpic(SolutionId, CapabilityId, EpicId, StatusId, LastUpdated, LastUpdatedBy)
         SELECT SolutionId, CapabilityId, EpicId, StatusId, LastUpdated, LastUpdatedBy
           FROM dbo.SolutionEpic;

    /*-----------------------------------------------------------------------
        Drop pricing tables from original schema
    ------------------------------------------------------------------------*/
    DROP TABLE IF EXISTS dbo.AdditionalServicePrice;
    DROP TABLE IF EXISTS dbo.AssociatedServicePrice;
    DROP TABLE IF EXISTS dbo.SolutionPrice;
    DROP TABLE IF EXISTS dbo.PurchasingModel;

    /*-----------------------------------------------------------------------
        Delete existing solution data
    ------------------------------------------------------------------------*/

    DELETE FROM dbo.Solution;

    /*-----------------------------------------------------------------------
        Delete any existing associated service data
    ------------------------------------------------------------------------*/
    IF OBJECT_ID(N'dbo.AssociatedService', N'U') IS NOT NULL
        TRUNCATE TABLE dbo.AssociatedService;
GO
