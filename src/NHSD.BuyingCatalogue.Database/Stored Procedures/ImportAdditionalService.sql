CREATE PROCEDURE import.ImportAdditionalService
     @CatalogueItemId varchar(14),
     @ServiceName varchar(255),
     @ServiceSummary varchar(300),
     @ServiceDescription varchar(3000)
AS
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @emptyGuid AS uniqueidentifier = CAST(0x0 AS uniqueidentifier);
        DECLARE @solutionId AS varchar(14) = SUBSTRING(@CatalogueItemId, 1, CHARINDEX('A', @CatalogueItemId) - 1);
        DECLARE @supplierId AS varchar(6) = SUBSTRING(@CatalogueItemId, 1, CHARINDEX('-', @CatalogueItemId) - 1);
        DECLARE @now AS datetime = GETUTCDATE();

        IF NOT EXISTS (SELECT * FROM dbo.Solution WHERE Id = @solutionId)
           THROW 51000, 'Parent Solution record does not exist.', 1;
        
        DECLARE @draftPublicationStatus AS int = (SELECT Id FROM dbo.PublicationStatus WHERE [Name] = 'Draft');
        DECLARE @additionalServiceCatalogueItemType AS int = (SELECT CatalogueItemTypeId FROM dbo.CatalogueItemType WHERE [Name] = 'Additional Service');

        IF NOT EXISTS (SELECT * FROM dbo.CatalogueItem WHERE CatalogueItemId = @CatalogueItemId)
            INSERT INTO dbo.CatalogueItem(CatalogueItemId, [Name], Created,
                        CatalogueItemTypeId, SupplierId, PublishedStatusId)
                 VALUES (@CatalogueItemId, @ServiceName, @now,
                        @additionalServiceCatalogueItemType, @supplierId, @draftPublicationStatus);

        IF NOT EXISTS (SELECT * FROM dbo.AdditionalService WHERE CatalogueItemId = @CatalogueItemId)
            INSERT INTO dbo.AdditionalService(CatalogueItemId, SolutionId, Summary, FullDescription,
                   LastUpdated, LastUpdatedBy)
            VALUES (@CatalogueItemId, @solutionId, @ServiceSummary, @ServiceDescription,
                   @now, @emptyGuid);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
