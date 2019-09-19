using System;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests.Entities
{
    public sealed class CapabilityEntity
    {
        public Guid Id { get; set; }

        public string CapabilityRef { get; set; }

        public string Version { get; set; }

        public string PreviousVersion { get; set; }

        public int StatusId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SourceUrl { get; set; }

        public DateTime EffectiveDate { get; set; }

        public int CategoryId { get; set; }

        public async Task Insert()
        {
            await SqlRunner.ExecuteAsync(Database.ConnectionString, InsertSql);
        }

        private string InsertSql => @$"
        INSERT INTO [dbo].[Capability]
        ([Id]
        ,[CapabilityRef]
        ,[Version]
        ,[PreviousVersion]
        ,[StatusId]
        ,[Name]
        ,[Description]
        ,[SourceUrl]
        ,[EffectiveDate]
        ,[CategoryId])

        VALUES
            ('{Id}'
            ,'{CapabilityRef}'
            ,'{Version}'
            ,'{PreviousVersion}'
            ,{StatusId}
            ,'{Name}'
            ,'{Description}'
            ,'{SourceUrl}'
            ,'{EffectiveDate.ToString("dd-MMM-yyyy")}'
            ,{CategoryId})";
    }
}
