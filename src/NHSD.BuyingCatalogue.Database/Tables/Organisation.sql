CREATE TABLE dbo.Organisation
(
     Id uniqueidentifier NOT NULL,
     [Name] varchar(255) NOT NULL,
     OdsCode varchar(8) NULL,
     PrimaryRoleId varchar(8) NULL,
     CrmRef uniqueidentifier NULL,
     [Address] nvarchar(500) NULL,
     CatalogueAgreementSigned bit CONSTRAINT DF_Organisation_CatalogueAgreement DEFAULT 0 NOT NULL,
     Deleted bit CONSTRAINT DF_Organisation_Deleted DEFAULT 0 NOT NULL,
     LastUpdated datetime2(7) NOT NULL,
     LastUpdatedBy uniqueidentifier NOT NULL,
     CONSTRAINT PK_Organisation PRIMARY KEY NONCLUSTERED (Id),
     INDEX IX_OrganisationName CLUSTERED ([Name])
);
