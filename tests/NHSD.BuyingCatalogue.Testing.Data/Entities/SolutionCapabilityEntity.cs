using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            ,[LastUpdated]
            ,[LastUpdatedBy]
        )
        VALUES
        (
             @SolutionId
            ,@CapabilityId
            ,@StatusId
            ,@LastUpdated
            ,@LastUpdatedBy
        )";

        public static async Task<IEnumerable<string>> FetchForSolutionAsync(string solutionId)
        {
            return await SqlRunner.FetchAllAsync<string>($@"SELECT
                                   [Capability].[CapabilityRef]
                                   FROM SolutionCapability
                                   INNER JOIN Capability ON SolutionCapability.CapabilityId = Capability.Id
                                   WHERE SolutionId = @solutionId;", new
            {
                solutionId,
            })
                .ConfigureAwait(false);
        }
    }
}
