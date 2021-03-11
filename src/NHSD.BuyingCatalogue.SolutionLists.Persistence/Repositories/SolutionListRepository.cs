using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;
using NHSD.BuyingCatalogue.SolutionLists.Persistence.Models;

namespace NHSD.BuyingCatalogue.SolutionLists.Persistence.Repositories
{
    /// <summary>
    /// Represents the data access layer for the Solution entity.
    /// </summary>
    public sealed class SolutionListRepository : ISolutionListRepository
    {
        private const string Sql = @"
            SELECT DISTINCT 
                   ci.CatalogueItemId AS SolutionId, ci.[Name] AS SolutionName, sol.Summary AS SolutionSummary,
                   sup.Id AS SupplierId, sup.[Name] AS SupplierName,
                   cap.CapabilityRef AS CapabilityReference, cap.[Name] AS CapabilityName, cap.[Description] as CapabilityDescription,
                   fs.IsFoundation AS IsFoundation,
                   fs.FrameworkId AS FrameworkId
              FROM dbo.CatalogueItem AS ci
                   INNER JOIN dbo.Solution AS sol
                           ON sol.Id = ci.CatalogueItemId
                   INNER JOIN dbo.Supplier AS sup
                           ON sup.Id = ci.SupplierId
                   INNER JOIN dbo.PublicationStatus AS ps
                           ON ps.Id = ci.PublishedStatusId
                   INNER JOIN dbo.SolutionCapability AS sc
                           ON sol.Id = sc.SolutionId
                   INNER JOIN dbo.Capability AS cap
                           ON cap.Id = sc.CapabilityId
                   INNER JOIN dbo.FrameworkSolutions AS fs
                           ON sol.Id = fs.SolutionId                             
             WHERE ps.[Name] = 'Published'
               AND fs.IsFoundation = ISNULL(@foundationOnly, fs.IsFoundation)
               AND ci.SupplierId = ISNULL(@supplierId, ci.SupplierId)
               AND fs.FrameworkId = ISNULL(@frameworkId, fs.FrameworkId);";

        private readonly IDbConnector dbConnector;

        public SolutionListRepository(IDbConnector dbConnector) => this.dbConnector = dbConnector;

        /// <summary>
        /// Gets a list of <see cref="ISolutionListResult"/> objects.
        /// </summary>
        /// <param name="foundationOnly">Specify <see langword="true"/> to include foundation solutions only.</param>
        /// <param name="supplierId">The ID of the supplier.</param>
        /// <param name="frameworkId">The ID of the framework.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the
        /// task to complete.</param>
        /// <returns>A list of <see cref="ISolutionListResult"/> objects.</returns>
        public async Task<IEnumerable<ISolutionListResult>> ListAsync(
            bool foundationOnly,
            string supplierId,
            string frameworkId,
            CancellationToken cancellationToken)
        {
            supplierId = string.IsNullOrWhiteSpace(supplierId) ? null : supplierId;

            return await dbConnector.QueryAsync<SolutionListResult>(
                Sql,
                cancellationToken,
                new { foundationOnly = foundationOnly ? (bool?)true : null, supplierId, frameworkId });
        }
    }
}
