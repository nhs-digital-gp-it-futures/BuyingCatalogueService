using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence;
using NHSD.BuyingCatalogue.Capabilities.Persistence.Models;
using NHSD.BuyingCatalogue.Data.Infrastructure;

namespace NHSD.BuyingCatalogue.Capabilities.Persistence.Repositories
{
    /// <summary>
    /// Represents the data access layer for the <see cref="ICapabilityListResult"/> entity.
    /// </summary>
    public sealed class CapabilityRepository : ICapabilityRepository
    {
        private const string Sql = @"
            SELECT c.CapabilityRef AS CapabilityReference,
                   c.[Version],
                   c.[Name],
                   ISNULL(f.IsFoundation, 0) AS IsFoundation
              FROM dbo.Capability AS c
              LEFT OUTER JOIN dbo.FrameworkCapabilities AS f
                   ON c.Id = f.CapabilityId
              WHERE c.CategoryId = 1
          ORDER BY UPPER(c.[Name]);";

        private readonly IDbConnector dbConnector;

        public CapabilityRepository(IDbConnector dbConnector) => this.dbConnector = dbConnector;

        /// <summary>
        /// Gets a list of <see cref="ICapabilityListResult"/> objects.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the
        /// task to complete.</param>
        /// <returns>A task representing an operation to retrieve a list of <see cref="ICapabilityListResult"/> objects.</returns>
        public async Task<IEnumerable<ICapabilityListResult>> ListAsync(CancellationToken cancellationToken) =>
            await dbConnector.QueryAsync<CapabilityListResult>(Sql, cancellationToken);
    }
}
