CREATE TABLE dbo.Framework
(
     Id varchar(10) NOT NULL,
     [Name] varchar(100) NOT NULL,
     [Description] varchar(max) NULL,
     [Owner] varchar(100) NULL,
     ActiveDate date NULL,
     ExpiryDate date NULL,
     CONSTRAINT PK_Framework PRIMARY KEY CLUSTERED (Id)
);
