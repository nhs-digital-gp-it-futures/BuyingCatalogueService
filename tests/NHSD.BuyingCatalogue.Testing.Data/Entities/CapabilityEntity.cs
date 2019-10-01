using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class CapabilityEntity : EntityBase
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

        protected override string InsertSql  => $@"
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

        public static async Task<IEnumerable<CapabilityEntity>> FetchAllAsync()
        {
            return await SqlRunner.FetchAllAsync<CapabilityEntity>($@"SELECT [Id]
                                ,[CapabilityRef]
                                ,[Version]
                                ,[PreviousVersion]
                                ,[StatusId]
                                ,[Name]
                                ,[Description]
                                ,[SourceUrl]
                                ,[EffectiveDate]
                                ,[CategoryId]
                                FROM Capability");
        }
    }
}
