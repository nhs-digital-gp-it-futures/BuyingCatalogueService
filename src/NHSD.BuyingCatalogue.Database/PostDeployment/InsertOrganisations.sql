DECLARE @emptyGuid AS uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @now AS datetime = GETUTCDATE();

IF UPPER('$(INSERT_TEST_DATA)') = 'TRUE' AND NOT EXISTS (SELECT * FROM dbo.Organisation)
    INSERT INTO dbo.Organisation(Id, [Name], PrimaryRoleId, CatalogueAgreementSigned, LastUpdated, LastUpdatedBy)
    VALUES ('f8e6e129-d69e-4368-8f50-f690f3940fa8', 'Docability Software', 'RO92', 1, @now, @emptyGuid);
GO
