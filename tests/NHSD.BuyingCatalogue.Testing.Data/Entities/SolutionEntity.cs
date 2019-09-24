
namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class SolutionEntity : EntityBase
    {
        public string Id { get; set; }
        public string OrganisationId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public int PublishedStatusId { get; set; }
        public int AuthorityStatusId { get; set; }
        public int SupplierStatusId { get; set; }
        public string ParentId { get; set; }
        public int OnCatalogueVersion { get; set; }
        public string Summary { get; set; }
        public string FullDescription { get; set; }

        protected override string InsertSql => $@"
        INSERT INTO [dbo].[Solution]
        (
            [Id]
            ,[OrganisationId]
            ,[Name]
	        ,[Version]
	        ,[PublishedStatusId]
	        ,[AuthorityStatusId]
	        ,[SupplierStatusId]
	        ,[ParentId]
	        ,[OnCatalogueVersion]
	        ,[Summary]
	        ,[FullDescription]
        )
        VALUES
        (
             '{Id}'
            ,'{OrganisationId}'
            ,'{Name}'
            ,'{Version}'
            ,{PublishedStatusId}
            ,{AuthorityStatusId}
            ,{SupplierStatusId}
            ,'{ParentId}'
            ,{OnCatalogueVersion}
            ,'{Summary}'
            ,'{FullDescription}'
        )";
    }
}
