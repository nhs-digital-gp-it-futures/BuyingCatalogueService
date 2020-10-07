CREATE TABLE dbo.PricingUnit
(
    PricingUnitId uniqueidentifier NOT NULL,
    [Name] varchar(20) NOT NULL,
    TierName varchar(25) NOT NULL,
    [Description] varchar(35) NOT NULL,
    CONSTRAINT PK_PricingUnit PRIMARY KEY NONClUSTERED (PricingUnitId)
);
