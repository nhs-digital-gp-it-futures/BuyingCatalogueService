using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    public sealed class SolutionEpicRepository : ISolutionEpicRepository
    {
        private readonly IDbConnector _dbConnector;

        public SolutionEpicRepository(IDbConnector dbConnector) =>
            _dbConnector = dbConnector;

        private const string DeleteSolutionEpic = @"DELETE FROM dbo.SolutionEpic WHERE SolutionId = @solutionId";
        private const string InsertSolutionEpic = @"INSERT INTO dbo.SolutionEpic (SolutionId, CapabilityId, EpicId, StatusId, LastUpdated, LastUpdatedBy) VALUES (@solutionId, (SELECT Epic.CapabilityId FROM Epic WHERE Epic.Id = @epicId), @epicId, (SELECT SolutionEpicStatus.Id FROM SolutionEpicStatus WHERE SolutionEpicStatus.Name = @statusName), GETDATE(), @lastUpdatedBy)";

        public async Task UpdateSolutionEpicAsync(string solutionId, IUpdateClaimedRequest request, CancellationToken cancellationToken)
        {
            request = request.ThrowIfNull(nameof(request));

            var lastUpdatedBy = new Guid();

            var queries = new List<(string, object)> { (DeleteSolutionEpic, new { solutionId }) };

            queries.AddRange(request.ClaimedEpics.Select(claimedEpic =>
                (insertSolutionEpic: InsertSolutionEpic,
                    (object)new
                    {
                        solutionId,
                        epicId = claimedEpic.EpicId,
                        statusName = claimedEpic.StatusName,
                        lastUpdatedBy
                    })));

            await _dbConnector.ExecuteMultipleWithTransactionAsync(queries, cancellationToken).ConfigureAwait(false);
        }
    }
}
