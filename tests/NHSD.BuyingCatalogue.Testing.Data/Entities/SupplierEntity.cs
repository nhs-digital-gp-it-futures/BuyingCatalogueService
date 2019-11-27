using System;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class SupplierEntity : EntityBase
    {
        public string Id { get; set; }
        public Guid OrganisationId { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string SupplierUrl { get; set; }
        public Guid? CrmRef { get; set; }
        public DateTime LastUpdated { get; set; }
        public Guid LastUpdatedBy { get; set; }

        protected override string InsertSql => $@"
            INSERT INTO [dbo].[Supplier]
            ([Id]
            ,[OrganisationId]
            ,[Name]
            ,[Summary]
            ,[SupplierUrl]
            ,[CrmRef]
            ,[LastUpdated]
            ,[LastUpdatedBy])
            VALUES
            ('{this.Id}'
            ,'{this.OrganisationId}'
            ,'{this.Name}'
            ,{NullOrWrapQuotes(this.Summary)}
            ,{NullOrWrapQuotes(this.SupplierUrl)}
            ,{NullOrWrapQuotes(this.CrmRef?.ToString())}
            ,'{LastUpdated.ToString("dd-MMM-yyyy")}'
            ,'{LastUpdatedBy}'
            )
        ";
    }
}
