IF NOT EXISTS (SELECT * FROM dbo.CapabilityStatus)
    INSERT INTO dbo.CapabilityStatus(Id, [Name])
    VALUES (1, 'Effective');
GO

IF NOT EXISTS (SELECT * FROM dbo.CapabilityCategory)
    INSERT INTO dbo.CapabilityCategory(Id, [Name])
    VALUES (0, 'Undefined');
GO

IF NOT EXISTS (SELECT * FROM dbo.CompliancyLevel)
    INSERT INTO dbo.CompliancyLevel(Id, [Name])
    VALUES
    (1, 'MUST'),
    (2, 'SHOULD'),
    (3, 'MAY');
GO

IF NOT EXISTS (SELECT * FROM dbo.PublicationStatus)
    INSERT INTO dbo.PublicationStatus(Id, [Name])
    VALUES
    (1, 'Draft'),
    (2, 'Unpublished'),
    (3, 'Published'),
    (4, 'Withdrawn');
GO

IF NOT EXISTS (SELECT * FROM dbo.SolutionSupplierStatus)
    INSERT INTO dbo.SolutionSupplierStatus(Id, [Name])
    VALUES
    (1, 'Draft'),
    (2, 'Authority Review'),
    (3, 'Completed');
GO

IF NOT EXISTS (SELECT * FROM dbo.SolutionAuthorityStatus)
    INSERT INTO dbo.SolutionAuthorityStatus(Id, [Name])
    VALUES
    (1, 'Draft'),
    (2, 'Completed');
GO

IF NOT EXISTS (SELECT * FROM dbo.SolutionCapabilityStatus)
    INSERT INTO dbo.SolutionCapabilityStatus(Id, [Name], Pass)
    VALUES
    (1, 'Passed – Full', 1),
    (2, 'Passed – Partial', 1),
    (3, 'Failed', 0);
GO

IF NOT EXISTS (SELECT * FROM dbo.SolutionEpicStatus)
    INSERT INTO dbo.SolutionEpicStatus(Id, [Name], IsMet)
    VALUES
    (1, 'Passed', 1),
    (2, 'Not Evidenced', 0),
    (3, 'Failed', 0);
GO
