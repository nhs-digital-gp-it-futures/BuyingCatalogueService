DECLARE @frameworks AS TABLE
(
    [Id]          NVARCHAR (10)  NOT NULL PRIMARY KEY,
    [Name]        NVARCHAR (100) NOT NULL,
    [Owner]       NVARCHAR (100) NULL
);

INSERT INTO @frameworks (Id, [Name], [Owner])
VALUES
('NHSDGP001', 'NHS Digital GP IT Futures Framework 1', 'NHS Digital'),
('DFOCVC001', 'Digital First Online Consultation and Video Consultation Framework 1', 'NHS England');

MERGE INTO dbo.Framework AS TARGET
    USING @frameworks AS SOURCE
        ON TARGET.Id = SOURCE.Id
    WHEN MATCHED THEN
        UPDATE SET TARGET.[Name] = SOURCE.[Name]
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (Id, [Name], [Owner])
        VALUES (SOURCE.[Id], SOURCE.[Name], SOURCE.[Owner]);
GO
