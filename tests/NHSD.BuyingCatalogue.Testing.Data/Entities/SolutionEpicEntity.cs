using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class SolutionEpicEntity : EntityBase
    {
        public string SolutionId { get; set; }

        public Guid CapabilityId { get; set; }

        public string EpicId { get; set; }

        public int StatusId { get; set; }

        protected override string InsertSql => @"
            INSERT INTO dbo.SolutionEpic
            (
                [SolutionId],
                [CapabilityId],
                [EpicId],
                [StatusId],
                [LastUpdated],
                [LastUpdatedBy]
            )
            VALUES
            (
                @SolutionId,
                @CapabilityId,
                @EpicId,
                @StatusId,
                @LastUpdated,
                @LastUpdatedBy
            );";

        public static async Task<IEnumerable<string>> FetchAllEpicIdsForSolutionAsync(string solutionId)
        {
            const string selectSql = @"
                SELECT EpicId
                  FROM dbo.SolutionEpic
                 WHERE SolutionId = @solutionId;";

            return await SqlRunner.FetchAllAsync<string>(selectSql, new { solutionId });
        }
    }
}
