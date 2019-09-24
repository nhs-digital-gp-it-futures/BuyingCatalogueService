using System;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class SolutionCapabilityEntity : EntityBase
    {
        public string SolutionId { get; set; }
        public Guid CapabilityId { get; set; }
        public int StatusId { get; set; }

        protected override string InsertSql => $@"
        INSERT INTO [dbo].[SolutionCapability]
        (
            [SolutionId]
            ,[CapabilityId]
            ,[StatusId]
        )
        VALUES
        (
             '{SolutionId}'
            ,'{CapabilityId}'
            ,{StatusId}
        )";
    }
}
