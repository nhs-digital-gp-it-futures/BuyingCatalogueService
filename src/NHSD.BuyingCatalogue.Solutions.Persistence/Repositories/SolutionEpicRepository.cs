using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    public sealed class SolutionEpicRepository : ISolutionEpicRepository
    {
        private readonly IDbConnector _dbConnector;

        public SolutionEpicRepository(IDbConnector dbConnector) =>
            _dbConnector = dbConnector;

        private const string DeleteSolutionEpic = @"DELETE FROM dbo.SolutionEpic WHERE SolutionId = @solutionId";
        private const string InsertSolutionEpic = @"INSERT INTO dbo.SolutionEpic (SolutionId, CapabilityId, EpicId, StatusId, LastUpdated, LastUpdatedBy) VALUES (@solutionId, (SELECT Epic.CapabilityId FROM Epic WHERE Epic.Id = @epicId), @epicId, (SELECT SolutionEpicStatus.Id FROM SolutionEpicStatus WHERE SolutionEpicStatus.Name = @statusName), GETDATE(), @lastUpdatedBy)";


        private const string Sql = @"SELECT
		               Epic.[Id] as EpicId
                      ,Epic.[Name] as EpicName
                      ,Epic.[CapabilityId]  as CapabilityId              
	                  ,CompliancyLevel.Name as EpicCompliancyLevel
	                  ,SolutionEpicStatus.IsMet as IsMet
                  FROM [Epic] 
                      inner join [CompliancyLevel] on Epic.CompliancyLevelId = CompliancyLevel.Id
                      inner join [SolutionEpic] on SolutionEpic.EpicId = Epic.Id and SolutionEpic.CapabilityId = Epic.CapabilityId
                      inner join [SolutionEpicStatus] on SolutionEpicStatus.Id = SolutionEpic.StatusId
                  Where [Epic].Active = 1 and [SolutionEpic].SolutionId = @solutionId
                  Order By EpicId";

        
        public async Task<IEnumerable<ISolutionEpicListResult>> ListSolutionEpicsAsync(string solutionId, CancellationToken cancellationToken)
            => await _dbConnector.QueryAsync<SolutionEpicListResult>(Sql, cancellationToken, new { solutionId })
                .ConfigureAwait(false);

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
