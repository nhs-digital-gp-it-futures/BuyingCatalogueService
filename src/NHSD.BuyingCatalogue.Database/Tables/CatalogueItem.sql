CREATE TABLE dbo.CatalogueItem
(
	CatalogueItemId varchar(14) NOT NULL,
    [Name] varchar(255),
    Created datetime2(7),
    CatalogueItemTypeId int,
    SupplierId varchar(6),
    PublishedStatusId int,
    CONSTRAINT PK_CatalogueItem PRIMARY KEY (CatalogueItemId),
    CONSTRAINT FK_CatalogueItem_CatalogueItemType FOREIGN KEY (CatalogueItemTypeId) REFERENCES CatalogueItemType(CatalogueItemTypeId),
    CONSTRAINT FK_CatalogueItem_Supplier FOREIGN KEY (SupplierId) REFERENCES Supplier(Id),
    CONSTRAINT FK_CatalogueItem_PublishedStatus FOREIGN KEY (PublishedStatusId) REFERENCES PublicationStatus(Id)
);
