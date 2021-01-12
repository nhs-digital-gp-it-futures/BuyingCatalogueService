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

        private const string Sql = @"SELECT Capability.Id as CapabilityId,
                                        Capability.Name as CapabilityName,
                                        Capability.Description as CapabilityDescription,
                                        Capability.Version as CapabilityVersion,
                                        Capability.SourceUrl as CapabilitySourceUrl
                                FROM SolutionCapability
                                     INNER JOIN Capability ON SolutionCapability.CapabilityId = Capability.Id
                                     INNER JOIN SolutionCapabilityStatus ON SolutionCapabilityStatus.Id = SolutionCapability.StatusId
                                WHERE SolutionCapability.SolutionId = @solutionId AND SolutionCapabilityStatus.Pass = 1
                                ORDER BY Capability.Name";

        private const string CheckCapabilitiesExist = @"SELECT COUNT(*)
                                                        FROM
                                                       (SELECT CapabilityRef
                                                        FROM Capability
                                                        WHERE CapabilityRef in @capabilitiesToMatch
                                                        GROUP BY CapabilityRef) AS count";

        private const string UpdateCapabilities = @"DELETE FROM SolutionCapability WHERE SolutionId = @solutionId
                                                    INSERT INTO dbo.SolutionCapability
                                                    (SolutionId, CapabilityId, StatusId, LastUpdated, LastUpdatedBy)
                                                    SELECT @solutionId AS SolutionId, Id, @statusId, GETUTCDATE(), @lastUpdatedBy
                                                    FROM Capability 
                                                    WHERE CapabilityRef in @newCapabilitiesReference";

        private readonly IDbConnector _dbConnector;

        public SolutionCapabilityRepository(IDbConnector dbConnector) => _dbConnector = dbConnector ?? throw new ArgumentNullException(nameof(dbConnector));

        public async Task<IEnumerable<ISolutionCapabilityListResult>> ListSolutionCapabilitiesAsync(string solutionId,
            CancellationToken cancellationToken)
            => await _dbConnector.QueryAsync<SolutionCapabilityListResult>(Sql, cancellationToken, new {solutionId})
                .ConfigureAwait(false);

        public async Task<int> GetMatchingCapabilitiesCountAsync(IEnumerable<string> capabilitiesToMatch,
            CancellationToken cancellationToken)
        {
            return (await _dbConnector
                .QueryAsync<int>(CheckCapabilitiesExist, cancellationToken, new {capabilitiesToMatch})
                .ConfigureAwait(false)).FirstOrDefault();
        }

        public async Task UpdateCapabilitiesAsync(IUpdateCapabilityRequest updateCapabilityRequest,
            CancellationToken cancellationToken)
        {
            if (updateCapabilityRequest is null)
            {
                throw new ArgumentNullException(nameof(updateCapabilityRequest));
            }

            await _dbConnector.ExecuteAsync(UpdateCapabilities, cancellationToken,
                    new
                    {
                        solutionId = updateCapabilityRequest.SolutionId,
                        newCapabilitiesReference = updateCapabilityRequest.NewCapabilitiesReference,
                        statusId = PassedFullCapabilityStatus,
                        lastUpdatedBy = Guid.NewGuid(),
                    })
                .ConfigureAwait(false);
        }
    }
}
