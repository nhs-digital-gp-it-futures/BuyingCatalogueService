DECLARE @frameworks AS TABLE
(
    Id nvarchar(10) NOT NULL PRIMARY KEY,
    [Name] nvarchar(100) NOT NULL,
    [Owner] nvarchar(100) NULL
);

INSERT INTO @frameworks (Id, [Name], [Owner])
VALUES
('NHSDGP001', 'NHS Digital GP IT Futures Framework 1', 'NHS Digital'),
('DFOCVC001', 'Digital First Online Consultation and Video Consultation Framework 1', 'NHS England');

MERGE INTO dbo.Framework AS TARGET
     USING @frameworks AS SOURCE ON TARGET.Id = SOURCE.Id
      WHEN MATCHED THEN
           UPDATE SET TARGET.[Name] = SOURCE.[Name]
      WHEN NOT MATCHED BY TARGET THEN
           INSERT (Id, [Name], [Owner])
           VALUES (SOURCE.Id, SOURCE.[Name], SOURCE.[Owner]);
GO
