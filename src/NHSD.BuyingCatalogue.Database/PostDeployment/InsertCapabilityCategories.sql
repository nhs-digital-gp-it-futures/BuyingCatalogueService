IF NOT EXISTS (SELECT * FROM dbo.CapabilityCategory)
    INSERT INTO dbo.CapabilityCategory(Id, [Name])
    VALUES (0, 'Undefined');
GO
