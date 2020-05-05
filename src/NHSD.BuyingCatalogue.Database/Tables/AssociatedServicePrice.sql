CREATE TABLE dbo.AssociatedServicePrice
(
     Id uniqueidentifier NOT NULL,
     PurchasingModelId uniqueidentifier NOT NULL,
     AssociatedServiceId varchar(18) NOT NULL,
     UnitId int NOT NULL,
     PriceTypeId int NOT NULL,
     ConsumptionPrice bit CONSTRAINT DF_AssociatedService_ConsumptionPrice DEFAULT 0 NOT NULL,
     [Description] varchar(30) NULL,
     Price decimal(18, 4) NOT NULL,
     BandStart int NOT NULL,
     BandEnd int NULL,
     Created datetime2(7) CONSTRAINT DF_AssociatedServicePrice_Created DEFAULT GETUTCDATE() NOT NULL,
     CONSTRAINT PK_AssociatedServicePrice PRIMARY KEY NONCLUSTERED (Id),
     CONSTRAINT FK_AssociatedServicePrice_PriceType FOREIGN KEY (PriceTypeId) REFERENCES dbo.PriceType(Id),
     CONSTRAINT FK_AssociatedServicePrice_PurchasingModelId FOREIGN KEY (PurchasingModelId) REFERENCES dbo.PurchasingModel(Id) ON DELETE CASCADE,
     CONSTRAINT FK_AssociateServicePrice_PricingUnit FOREIGN KEY (UnitId) REFERENCES dbo.PricingUnit(Id),
     INDEX IX_AssociatedServicePriceLastUpdated CLUSTERED (Created)
);
