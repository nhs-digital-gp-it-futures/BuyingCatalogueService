using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class SolutionEntity : EntityBase
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string SupplierId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid SolutionDetailId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public int PublishedStatusId { get; set; }
        public int AuthorityStatusId { get; set; }
        public int SupplierStatusId { get; set; }
        public int OnCatalogueVersion { get; set; }
        public string ServiceLevelAgreement { get; set; }
        public string WorkOfPlan { get; set; }
        public DateTime LastUpdated { get; set; }
        public Guid LastUpdatedBy { get; set; }


        protected override string InsertSql => $@"
        INSERT INTO [dbo].[Solution]
        (
            [Id]
            ,[ParentId]
            ,[SupplierId]
            ,[OrganisationId]
            ,[SolutionDetailId]
            ,[Name]
	        ,[Version]
	        ,[PublishedStatusId]
	        ,[AuthorityStatusId]
	        ,[SupplierStatusId]
	        ,[OnCatalogueVersion]
	        ,[ServiceLevelAgreement]
	        ,[WorkOfPlan]
	        ,[LastUpdated]
	        ,[LastUpdatedBy]
        )
        VALUES
        (
             '{Id}'
            ,'{ParentId}'
            ,'{SupplierId}'
            ,'{OrganisationId}'
            ,'{SolutionDetailId}'
            ,'{Name}'
            ,{NullOrWrapQuotes(Version)}
            ,'{PublishedStatusId}'
            ,'{AuthorityStatusId}'
            ,'{SupplierStatusId}'
            ,'{OnCatalogueVersion}'
            ,{NullOrWrapQuotes(ServiceLevelAgreement)}
            ,{NullOrWrapQuotes(WorkOfPlan)}
            ,'{LastUpdated}'
            ,'{LastUpdatedBy}'

        )";

        public static async Task<IEnumerable<SolutionEntity>> FetchAllAsync()
        {
            return await SqlRunner.FetchAllAsync<SolutionEntity>($@"SELECT [Id]
                                ,[ParentId]
                                ,[SupplierId]
                                ,[OrganisationId]
                                ,[SolutionDetailId]
                                ,[Name]
	                            ,[Version]
	                            ,[PublishedStatusId]
	                            ,[AuthorityStatusId]
	                            ,[SupplierStatusId]
	                            ,[OnCatalogueVersion]
	                            ,[ServiceLevelAgreement]
	                            ,[WorkOfPlan]
	                            ,[LastUpdated]
	                            ,[LastUpdatedBy]

                                FROM Solution");
        }

        public static async Task<SolutionEntity> GetByIdAsync(string solutionId)
        {
            return (await FetchAllAsync()).First(item => solutionId.Equals(item.Id));
        }
    }
}
