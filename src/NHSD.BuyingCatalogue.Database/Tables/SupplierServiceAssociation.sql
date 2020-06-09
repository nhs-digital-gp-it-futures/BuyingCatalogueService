CREATE TABLE dbo.SupplierServiceAssociation
(
    AssociatedServiceId varchar(14) NOT NULL,
    CatalogueItemId varchar(14) NOT NULL,

    CONSTRAINT FK_AssociatedService FOREIGN KEY (AssociatedServiceId) REFERENCES AssociatedService(AssociatedServiceId),
    CONSTRAINT FK_CatalogueItem FOREIGN KEY (CatalogueItemId) REFERENCES CatalogueItem(CatalogueItemId)
);
