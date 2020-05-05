CREATE TABLE dbo.Supplier
(
     Id varchar(6) NOT NULL,
     [Name] varchar(255) NOT NULL,
     LegalName varchar(255) NOT NULL,
     Summary varchar(1000) NULL,
     SupplierUrl varchar(1000) NULL,
     [Address] nvarchar(500) NULL,
     OdsCode varchar(8) NULL,
     CrmRef uniqueidentifier NULL,
     Deleted bit CONSTRAINT DF_Supplier_Deleted DEFAULT 0 NOT NULL,
     LastUpdated datetime2(7) NOT NULL,
     LastUpdatedBy uniqueidentifier NOT NULL,
     CONSTRAINT PK_Supplier PRIMARY KEY CLUSTERED (Id)
);
