CREATE TABLE dbo.AssociatedService
(
    AssociatedServiceId varchar(14) NOT NULL,
    [Description] varchar(1000) NULL,
    OrderGuidance varchar(1000) NULL,
    LastUpdated datetime2(7) NULL,
    LastUpdatedBy uniqueidentifier NULL,

    CONSTRAINT PK_AssociatedService PRIMARY KEY (AssociatedServiceId)
);
