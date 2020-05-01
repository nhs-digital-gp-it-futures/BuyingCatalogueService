CREATE TABLE dbo.AdditionalServiceDetail
(
     Id varchar(14) NOT NULL,
     FullDescription varchar(3000) NULL,
     LastUpdated datetime2(7) NOT NULL,
     LastUpdatedBy uniqueidentifier NOT NULL,
     CONSTRAINT PK_AdditionalServiceDetail PRIMARY KEY CLUSTERED (Id),
     CONSTRAINT FK_AdditionalServiceDetail_Solution_Id FOREIGN KEY (Id) REFERENCES dbo.Solution(Id)
);
