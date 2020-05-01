CREATE TABLE dbo.[Audit]
(
     Id uniqueidentifier NOT NULL,
     DataType varchar(50) NOT NULL,
     DataItemId varchar(50) NOT NULL,
     AuditType varchar(30) NOT NULL,
     PerformedAt datetime2(7) NOT NULL,
     PerformedBy uniqueidentifier NOT NULL,
     CONSTRAINT PK_Audit PRIMARY KEY NONCLUSTERED (Id),
     INDEX IX_AuditPerformedAt CLUSTERED (PerformedAt)
);
