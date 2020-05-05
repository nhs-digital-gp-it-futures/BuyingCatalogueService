CREATE TABLE dbo.SolutionDetail
(
     Id uniqueidentifier NOT NULL,
     SolutionId varchar(14) NOT NULL,
     PublishedStatusId int CONSTRAINT DF_SolutionDetail_PublishedStatus DEFAULT 1 NOT NULL,
     Features nvarchar(max) NULL,
     ClientApplication nvarchar(max) NULL,
     Hosting nvarchar(max) NULL,
     ImplementationDetail varchar(1000) NULL,
     RoadMap varchar(1000) NULL,
     IntegrationsUrl varchar(1000) NULL,
     AboutUrl varchar(1000) NULL,
     Summary varchar(300) NULL,
     FullDescription varchar(3000) NULL,
     LastUpdated datetime2(7) NOT NULL,
     LastUpdatedBy uniqueidentifier NOT NULL,
     CONSTRAINT PK_SolutionDetail PRIMARY KEY NONCLUSTERED (Id),
     CONSTRAINT FK_SolutionDetail_PublicationStatus FOREIGN KEY (PublishedStatusId) REFERENCES dbo.PublicationStatus(Id),
     CONSTRAINT FK_SolutionDetail_Solution FOREIGN KEY (SolutionId) REFERENCES dbo.Solution(Id) ON DELETE CASCADE,
     INDEX IX_SolutionDetail CLUSTERED (SolutionId)
);
