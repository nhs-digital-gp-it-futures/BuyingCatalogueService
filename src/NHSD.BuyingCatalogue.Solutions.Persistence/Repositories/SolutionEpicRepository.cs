using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    public sealed class SolutionEpicRepository : ISolutionEpicRepository
    {
        private const string DeleteSolutionEpicSql = @"DELETE FROM dbo.SolutionEpic WHERE SolutionId = @solutionId";

        private const string InsertSolutionEpicSql = @"
            INSERT INTO dbo.SolutionEpic (SolutionId, CapabilityId, EpicId, StatusId, LastUpdated, LastUpdatedBy)
                 VALUES (@solutionId,
                        (SELECT Epic.CapabilityId FROM dbo.Epic WHERE Epic.Id = @epicId), @epicId,
                        (SELECT SolutionEpicStatus.Id FROM dbo.SolutionEpicStatus WHERE SolutionEpicStatus.Name = @statusName),
                        GETUTCDATE(), @lastUpdatedBy);";

        private const string ListSolutionEpicsSql = @"
            SELECT Epic.Id AS EpicId,
                   Epic.[Name] AS EpicName,
                   Epic.CapabilityId AS CapabilityId,
	               CompliancyLevel.[Name] AS EpicCompliancyLevel,
	               SolutionEpicStatus.IsMet AS IsMet
              FROM dbo.Epic
                   INNER JOIN dbo.CompliancyLevel
                           ON Epic.CompliancyLevelId = CompliancyLevel.Id
                   INNER JOIN dbo.SolutionEpic
                           ON SolutionEpic.EpicId = Epic.Id and SolutionEpic.CapabilityId = Epic.CapabilityId
                   INNER JOIN dbo.SolutionEpicStatus
                           ON SolutionEpicStatus.Id = SolutionEpic.StatusId
             WHERE dbo.Epic.Active = 1 AND SolutionEpic.SolutionId = @solutionId
          ORDER BY EpicId;";

        private readonly IDbConnector dbConnector;

        public SolutionEpicRepository(IDbConnector dbConnector) =>
            this.dbConnector = dbConnector;

        public async Task<IEnumerable<ISolutionEpicListResult>> ListSolutionEpicsAsync(
            string solutionId,
            CancellationToken cancellationToken)
        {
            return await dbConnector.QueryAsync<SolutionEpicListResult>(
                ListSolutionEpicsSql,
                cancellationToken,
                new { solutionId });
        }

        public async Task UpdateSolutionEpicAsync(
            string solutionId,
            IUpdateClaimedEpicListRequest request,
            CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var lastUpdatedBy = Guid.Empty;

            var queries = new List<(string, object)> { (DeleteSolutionEpicSql, new { solutionId }) };

            queries.AddRange(request.ClaimedEpics.Select(claimedEpic =>
                (insertSolutionEpic: InsertSolutionEpicSql,
                    (object)new
                    {
                        solutionId,
                        epicId = claimedEpic.EpicId,
                        statusName = claimedEpic.StatusName,
                        lastUpdatedBy,
                    })));

            await dbConnector.ExecuteMultipleWithTransactionAsync(queries, cancellationToken);
        }
    }
}
