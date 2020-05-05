CREATE TABLE dbo.PricingUnit
(
     Id int NOT NULL,
     [Name] varchar(50) NOT NULL,
     [Description] varchar(500) NOT NULL,
     CONSTRAINT PK_PricingUnit PRIMARY KEY CLUSTERED (Id)
);
