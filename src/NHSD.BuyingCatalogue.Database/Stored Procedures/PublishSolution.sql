CREATE PROCEDURE publish.PublishSolution
     @SolutionId varchar(16)
AS
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @emptyGuid AS uniqueidentifier = CAST(0x0 AS uniqueidentifier);
        DECLARE @solutionDetailId AS uniqueidentifier;

        UPDATE dbo.Solution
           SET PublishedStatusId = 3,
               AuthorityStatusId = 2,
               SupplierStatusId = 3
         WHERE Id = @SolutionId;

        SELECT @solutionDetailId = SolutionDetailId
          FROM dbo.Solution;

        UPDATE dbo.SolutionDetail
           SET PublishedStatusId = 3
         WHERE Id = @solutionDetailId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
