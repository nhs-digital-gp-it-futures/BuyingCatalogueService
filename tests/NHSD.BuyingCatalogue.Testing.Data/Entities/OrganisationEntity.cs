using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{

    public sealed class OrganisationEntity : EntityBase
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public string OrganisationUrl { get; set; }

        public string OdsCode { get; set; }

        public string PrimaryRoleId { get; set; }

        public Guid CrmRef { get; set; }

        protected override string InsertSql => $@"
        INSERT INTO [dbo].[Organisation]
        ([Id]
        ,[Name]
        ,[Summary]
        ,[OrganisationUrl]
        ,[OdsCode]
        ,[PrimaryRoleId]
        ,[CrmRef])

        VALUES
            ('{Id}'
            ,'{Name}'
            ,{NullOrWrapQuotes(Summary)}
            ,{NullOrWrapQuotes(OrganisationUrl)}
            ,{NullOrWrapQuotes(OdsCode)}
            ,{NullOrWrapQuotes(PrimaryRoleId)}
            ,'{CrmRef}')";

        public static async Task<IEnumerable<OrganisationEntity>> FetchAllAsync()
        {
            return await SqlRunner.FetchAllAsync<OrganisationEntity>($@"SELECT [Id]
                                    ,[Name]
                                    ,[Summary]
                                    ,[OrganisationUrl]
                                    ,[OdsCode]
                                    ,[PrimaryRoleId]
                                    ,[CrmRef]
                                FROM Organisation");
        }
    }
}
