CREATE TABLE dbo.MarketingContact
(
     Id int IDENTITY(1, 1) NOT NULL,
     SolutionId varchar(14) NOT NULL,
     FirstName varchar(35) NULL,
     LastName varchar(35) NULL,
     Email varchar(255) NULL,
     PhoneNumber varchar(35) NULL,
     Department varchar(50) NULL,
     LastUpdated datetime2(7) NOT NULL,
     LastUpdatedBy uniqueidentifier NOT NULL,
     CONSTRAINT PK_MarketingContact PRIMARY KEY CLUSTERED (SolutionId, Id),
     CONSTRAINT FK_MarketingContact_Solution FOREIGN KEY (SolutionId) REFERENCES dbo.Solution(Id) ON DELETE CASCADE
);
