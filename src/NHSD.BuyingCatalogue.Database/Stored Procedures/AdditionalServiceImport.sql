CREATE PROCEDURE dbo.AdditionalServiceImport
     @AdditionalServiceId varchar(14),
     @ServiceName varchar(255),
     @ServiceSummary varchar(300),
     @ServiceDescription varchar(3000),
     @Capabilities dbo.AdditionalServiceImportCapability READONLY
AS
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @emptyGuid AS uniqueidentifier = CAST(0x0 AS uniqueidentifier);
        DECLARE @solutionDetailId AS uniqueidentifier;
        DECLARE @publishedStatusId AS int;
        DECLARE @authorityStatusId AS int;
        DECLARE @supplierStatusId AS int;
        DECLARE @parentSolutionId AS varchar(14) = SUBSTRING(@AdditionalServiceId, 1, CHARINDEX('A', @AdditionalServiceId) - 1);
        DECLARE @passedFull AS int = 1;
        DECLARE @supplierId AS varchar(6) = SUBSTRING(@parentSolutionId, 1, CHARINDEX('-', @parentSolutionId) - 1);

        IF NOT EXISTS (SELECT * FROM dbo.Solution WHERE Id = @parentSolutionId)
            THROW 51000, 'Parent Solution record does not exist.', 1;

        SELECT @publishedStatusId = PublishedStatusId,
               @authorityStatusId = AuthorityStatusId,
               @supplierStatusId = SupplierStatusId
          FROM dbo.Solution
         WHERE Id = @parentSolutionId;

        IF NOT EXISTS (SELECT * FROM dbo.Solution WHERE Id = @AdditionalServiceId)
            INSERT INTO dbo.Solution(Id, ParentId, SupplierId, [Name],
                   PublishedStatusId, AuthorityStatusId, SupplierStatusId,
                   LastUpdated, LastUpdatedBy)
            VALUES (@AdditionalServiceId, @parentSolutionId, @supplierId, @ServiceName,
                   @publishedStatusId, @authorityStatusId, @supplierStatusId,
                   GETUTCDATE(), @emptyGuid);

        UPDATE dbo.Solution
           SET [Name] = @ServiceName,
               PublishedStatusId = @publishedStatusId,
               AuthorityStatusId = @authorityStatusId,
               SupplierStatusId = @supplierStatusId
         WHERE Id = @AdditionalServiceId;

        IF NOT EXISTS
            (SELECT *
               FROM dbo.Solution AS s
                    INNER JOIN dbo.SolutionDetail AS sd
                    ON sd.SolutionId = s.Id
                    AND sd.Id = s.SolutionDetailId
              WHERE s.Id = @AdditionalServiceId)
        BEGIN
            SELECT @solutionDetailId = NEWID();

            INSERT INTO dbo.SolutionDetail(Id, SolutionId, Summary, FullDescription,
                   PublishedStatusId, LastUpdated, LastUpdatedBy)
            VALUES (@solutionDetailId, @AdditionalServiceId, @ServiceSummary, @ServiceDescription,
                   @publishedStatusId, GETUTCDATE(), @emptyGuid);

            UPDATE dbo.Solution
               SET SolutionDetailId = @solutionDetailId
             WHERE Id = @AdditionalServiceId;
        END;

        DELETE FROM dbo.SolutionCapability
              WHERE SolutionId = @AdditionalServiceId;

        INSERT INTO dbo.SolutionCapability
             SELECT @AdditionalServiceId AS SolutionId, c.Id AS CapabilityId, @passedFull AS StatusId,
                    GETUTCDATE() AS LastUpdated, @emptyGuid AS LastUpdatedBy
               FROM @Capabilities AS cin
                    INNER JOIN dbo.Capability AS c
                    ON c.CapabilityRef = cin.CapabilityRef;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
