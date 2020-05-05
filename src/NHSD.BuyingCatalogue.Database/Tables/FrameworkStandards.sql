CREATE TABLE dbo.FrameworkStandards
(
     FrameworkId varchar(10) NOT NULL,
     StandardId uniqueidentifier NOT NULL,
     CONSTRAINT PK_FrameworkStandards PRIMARY KEY CLUSTERED (FrameworkId, StandardId),
     CONSTRAINT FK_FrameworkStandards_Framework FOREIGN KEY (FrameworkId) REFERENCES dbo.Framework(Id),
     CONSTRAINT FK_FrameworkStandards_Standard FOREIGN KEY (StandardId) REFERENCES dbo.[Standard](Id)
);
