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
    public sealed class SolutionCapabilityRepository : ISolutionCapabilityRepository
    {
        private const int PassedFullCapabilityStatus = 1;

        private const string Sql = @"
            SELECT Capability.Id AS CapabilityId,
                   Capability.Name AS CapabilityName,
                   Capability.Description AS CapabilityDescription,
                   Capability.Version AS CapabilityVersion,
                   Capability.SourceUrl AS CapabilitySourceUrl
              FROM dbo.SolutionCapability
                   INNER JOIN dbo.Capability ON SolutionCapability.CapabilityId = Capability.Id
                   INNER JOIN dbo.SolutionCapabilityStatus ON SolutionCapabilityStatus.Id = SolutionCapability.StatusId
             WHERE SolutionCapability.SolutionId = @solutionId AND SolutionCapabilityStatus.Pass = 1
          ORDER BY Capability.Name;";

        private const string CheckCapabilitiesExist = @"
            SELECT COUNT(*)
              FROM (SELECT CapabilityRef
                     FROM dbo.Capability
                    WHERE CapabilityRef in @capabilitiesToMatch
                 GROUP BY CapabilityRef) AS count;";

        private const string UpdateCapabilities = @"
            DELETE FROM SolutionCapability
                  WHERE SolutionId = @solutionId;

            INSERT INTO dbo.SolutionCapability(SolutionId, CapabilityId, StatusId, LastUpdated, LastUpdatedBy)
                 SELECT @solutionId AS SolutionId, Id, @statusId, GETUTCDATE(), @lastUpdatedBy
                   FROM dbo.Capability
                  WHERE CapabilityRef in @newCapabilitiesReference;";

        private readonly IDbConnector dbConnector;

        public SolutionCapabilityRepository(IDbConnector dbConnector) => this.dbConnector = dbConnector
            ?? throw new ArgumentNullException(nameof(dbConnector));

        public async Task<IEnumerable<ISolutionCapabilityListResult>> ListSolutionCapabilitiesAsync(
            string solutionId,
            CancellationToken cancellationToken) =>
            await dbConnector.QueryAsync<SolutionCapabilityListResult>(Sql, cancellationToken, new { solutionId });

        public async Task<int> GetMatchingCapabilitiesCountAsync(
            IEnumerable<string> capabilitiesToMatch,
            CancellationToken cancellationToken)
        {
            return (await dbConnector.QueryAsync<int>(
                CheckCapabilitiesExist,
                cancellationToken,
                new { capabilitiesToMatch })).FirstOrDefault();
        }

        public async Task UpdateCapabilitiesAsync(
            IUpdateCapabilityRequest updateCapabilityRequest,
            CancellationToken cancellationToken)
        {
            if (updateCapabilityRequest is null)
            {
                throw new ArgumentNullException(nameof(updateCapabilityRequest));
            }

            await dbConnector.ExecuteAsync(
                UpdateCapabilities,
                cancellationToken,
                new
                {
                    solutionId = updateCapabilityRequest.SolutionId,
                    newCapabilitiesReference = updateCapabilityRequest.NewCapabilitiesReference,
                    statusId = PassedFullCapabilityStatus,
                    lastUpdatedBy = Guid.NewGuid(),
                });
        }
    }
}
