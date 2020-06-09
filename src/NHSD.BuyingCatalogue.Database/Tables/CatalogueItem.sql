CREATE TABLE dbo.CatalogueItem
(
	CatalogueItemId varchar(14) NOT NULL,
    [Name] varchar(255) NULL,
    Created datetime2(7) NULL,
    CatalogueItemTypeId int NULL,
    SupplierId varchar(6) NULL,
    PublishedStatusId int NULL,
    CONSTRAINT PK_CatalogueItem PRIMARY KEY (CatalogueItemId),
    CONSTRAINT FK_CatalogueItem_CatalogueItemType FOREIGN KEY (CatalogueItemTypeId) REFERENCES CatalogueItemType(CatalogueItemTypeId),
    CONSTRAINT FK_CatalogueItem_Supplier FOREIGN KEY (SupplierId) REFERENCES Supplier(Id),
    CONSTRAINT FK_CatalogueItem_PublishedStatus FOREIGN KEY (PublishedStatusId) REFERENCES PublicationStatus(Id)
);
