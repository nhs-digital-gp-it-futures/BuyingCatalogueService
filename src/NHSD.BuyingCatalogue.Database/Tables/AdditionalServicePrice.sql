CREATE TABLE dbo.AdditionalServicePrice
(
     Id uniqueidentifier NOT NULL,
     PurchasingModelId uniqueidentifier NOT NULL,
     AdditionalServiceId varchar(14) NOT NULL,
     UnitId int NOT NULL,
     PriceTypeId int NOT NULL,
     ConsumptionPrice bit CONSTRAINT DF_AdditionalService_ConsumptionPrice DEFAULT 0 NOT NULL,
     [Description] varchar(30) NULL,
     Price decimal(18, 4) NOT NULL,
     BandStart int NOT NULL,
     BandEnd int NULL,
     CONSTRAINT PK_AdditionalServicePrice PRIMARY KEY NONCLUSTERED (Id),
     CONSTRAINT FK_AdditionalServicePrice_PriceType FOREIGN KEY (PriceTypeId) REFERENCES dbo.PriceType(Id),
     CONSTRAINT FK_AdditionalServicePrice_PricingUnit FOREIGN KEY (UnitId) REFERENCES dbo.PricingUnit(Id),
     CONSTRAINT FK_AdditionalServicePrice_PurchasingModelId FOREIGN KEY (PurchasingModelId) REFERENCES dbo.PurchasingModel(Id) ON DELETE CASCADE,
     CONSTRAINT FK_AdditionalServicePrice_Soution FOREIGN KEY (AdditionalServiceId) REFERENCES dbo.Solution(Id),
     INDEX IX_AdditionalServicePriceAdditionalServiceId CLUSTERED (AdditionalServiceId)
);
