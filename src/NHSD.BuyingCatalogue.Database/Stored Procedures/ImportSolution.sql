CREATE PROCEDURE import.ImportSolution
     @SolutionId varchar(14),
     @SolutionName varchar(255),
     @IsFoundation bit,
     @Capabilities import.SolutionCapability READONLY
AS
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @draft AS int = 1;
        DECLARE @emptyGuid AS uniqueidentifier = CAST(0x0 AS uniqueidentifier);
        DECLARE @frameworkId AS varchar(10) = 'NHSDGP001';
        DECLARE @passedFull AS int = 1;
        DECLARE @solutionDetailId AS uniqueidentifier;
        DECLARE @supplierId AS varchar(6) = SUBSTRING(@SolutionId, 1, CHARINDEX('-', @SolutionId) - 1);

        IF NOT EXISTS (SELECT * FROM dbo.Supplier WHERE Id = @supplierId)
            THROW 51000, 'Supplier record does not exist.', 1;

        IF NOT EXISTS (SELECT * FROM dbo.Solution WHERE Id = @SolutionId)
            INSERT INTO dbo.Solution(Id, SupplierId, SolutionDetailId, [Name],
                   PublishedStatusId, AuthorityStatusId, SupplierStatusId,
                   OnCatalogueVersion, LastUpdated, LastUpdatedBy)
            VALUES (@SolutionId, @supplierId, NULL, @SolutionName,
                   @draft, @draft, @draft,
                   0, GETUTCDATE(), @emptyGuid);

        UPDATE dbo.Solution
           SET [Name] = @SolutionName
         WHERE Id = @SolutionId;

        IF NOT EXISTS (SELECT * FROM dbo.FrameworkSolutions WHERE SolutionId = @SolutionId AND FrameworkId = @frameworkId)
            INSERT INTO dbo.FrameworkSolutions(FrameworkId, SolutionId, IsFoundation,
                   LastUpdated, LastUpdatedBy)
            VALUES (@frameworkId, @SolutionId, 0,
                   GETUTCDATE(), @emptyGuid);

        UPDATE dbo.FrameworkSolutions
           SET IsFoundation = @IsFoundation
         WHERE SolutionId = @SolutionId
           AND FrameworkId = @frameworkId;

        IF NOT EXISTS
           (SELECT *
              FROM dbo.Solution AS s
                   INNER JOIN dbo.SolutionDetail AS sd
                   ON sd.SolutionId = s.Id
                   AND sd.Id = s.SolutionDetailId
             WHERE s.Id = @SolutionId)
        BEGIN
            SELECT @solutionDetailId = NEWID();
            INSERT INTO dbo.SolutionDetail(Id, SolutionId, PublishedStatusId,
                   LastUpdated, LastUpdatedBy)
            VALUES (@solutionDetailId, @SolutionId, @draft,
                   GETUTCDATE(), @emptyGuid);

            UPDATE dbo.Solution
               SET SolutionDetailId = @solutionDetailId
             WHERE Id = @SolutionId;
        END;

        DELETE FROM dbo.SolutionCapability
              WHERE SolutionId = @SolutionId;

        INSERT INTO dbo.SolutionCapability
             SELECT @SolutionId AS SolutionId, c.Id AS CapabilityId, @passedFull AS StatusId,
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
