using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class OrganisationEntity : EntityBase
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string OdsCode { get; set; }

        public string PrimaryRoleId { get; set; }

        public Guid CrmRef { get; set; }

        public string Address { get; set; }

        public bool CatalogueAgreementSigned { get; set; }

        public bool Deleted { get; set; }

        public DateTime LastUpdated { get; set; }

        public Guid LastUpdatedBy { get; set; }

        protected override string InsertSql => $@"
        INSERT INTO [dbo].[Organisation]
        ([Id]
        ,[Name]
        ,[OdsCode]
        ,[PrimaryRoleId]
        ,[CrmRef]
        ,[Address]
        ,[CatalogueAgreementSigned]
        ,[Deleted]
        ,[LastUpdated]
        ,[LastUpdatedBy])

        VALUES
            (@Id
            ,@Name
            ,@OdsCode
            ,@PrimaryRoleId
            ,@CrmRef
            ,@Address
            ,@CatalogueAgreementSigned
            ,@Deleted
            ,@LastUpdated
            ,@LastUpdatedBy)";

        public static async Task<IEnumerable<OrganisationEntity>> FetchAllAsync()
        {
            return await SqlRunner.FetchAllAsync<OrganisationEntity>($@"SELECT [Id]
                                    ,[Name]
                                    ,[OdsCode]
                                    ,[PrimaryRoleId]
                                    ,[CrmRef]
                                    ,[Address]
                                    ,[CatalogueAgreementSigned]
                                    ,[Deleted]
                                    ,[LastUpdated]
                                    ,[LastUpdatedBy]
                                FROM Organisation")
                .ConfigureAwait(false);
        }
    }
}
