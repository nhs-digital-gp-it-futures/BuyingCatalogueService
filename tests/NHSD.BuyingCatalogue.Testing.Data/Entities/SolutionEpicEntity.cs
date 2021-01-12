using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class SolutionEpicEntity : EntityBase
    {
        public string SolutionId { get; set; }

        public Guid CapabilityId { get; set; }

        public string EpicId { get; set; }

        public int StatusId { get; set; }

        protected override string InsertSql => $@"
        INSERT INTO [dbo].[SolutionEpic]
        (
            [SolutionId]
           ,[CapabilityId]
           ,[EpicId]
           ,[StatusId]
           ,[LastUpdated]
           ,[LastUpdatedBy]
        )
        VALUES
        (
            @SolutionId
           ,@CapabilityId
           ,@EpicId
           ,@StatusId
           ,@LastUpdated
           ,@LastUpdatedBy
        )";

        public static async Task<IEnumerable<string>> FetchAllEpicIdsForSolutionAsync(string solutionId)
        {
            return await SqlRunner.FetchAllAsync<string>($@"SELECT
                                   [EpicId]
                                   FROM SolutionEpic
                                   WHERE SolutionId = @solutionId;", new
                {
                    solutionId,
                })
                .ConfigureAwait(false);
        }
    }
}
