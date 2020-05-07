CREATE PROCEDURE test.ClearData AS
    SET NOCOUNT ON;

    TRUNCATE TABLE dbo.FrameworkCapabilities;
    TRUNCATE TABLE dbo.FrameworkSolutions;

    UPDATE dbo.Solution
    SET SolutionDetailId = NULL;

    DELETE FROM dbo.SolutionDetail;
    TRUNCATE TABLE dbo.SolutionEpic;
    TRUNCATE TABLE dbo.SolutionCapability;
    DELETE FROM dbo.Solution;
    DELETE FROM dbo.Epic;
    DELETE FROM dbo.Capability;
    DELETE FROM dbo.Supplier;
    TRUNCATE TABLE dbo.SupplierContact;

    ALTER ROLE Api
    ADD MEMBER NHSD;
GO
