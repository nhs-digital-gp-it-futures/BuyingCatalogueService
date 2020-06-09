/*-----------------------------------------------------------------------
    Copy Solution ID and Names to CatatlogueItem Table	
------------------------------------------------------------------------*/
DECLARE @solutionCatalogueItemType int = 1;

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    INSERT INTO dbo.CatalogueItem(CatalogueItemId, [Name], Created, CatalogueItemTypeId, SupplierId, PublishedStatusId)
         SELECT CatalogueItemId, [Name], Created, @solutionCatalogueItemType, SupplierId, PublishedStatusId
           FROM migration.CatalogueItem;
GO

/*-----------------------------------------------------------------------
    Copy Solution Detail information to Solution Table	
------------------------------------------------------------------------*/

IF UPPER('$(MIGRATE_TO_CATALOGUE_ITEM)') = 'TRUE'
    UPDATE s
       SET s.Summary = d.Summary,
           s.FullDescription = d.FullDescription,
           s.Features = d.Features,
           s.ClientApplication = d.ClientApplication,
           s.Hosting = d.Hosting,
           s.ImplementationDetail = d.ImplementationDetail,
           s.RoadMap = d.RoadMap,
           s.IntegrationsUrl = d.IntegrationsUrl,
           s.AboutUrl = d.AboutUrl
      FROM migration.SolutionDetail AS d
           INNER JOIN dbo.Solution AS s
           ON s.Id = d.SolutionId;
GO
