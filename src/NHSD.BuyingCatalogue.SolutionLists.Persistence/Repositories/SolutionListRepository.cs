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
        private readonly IDbConnector _dbConnector;

        public SolutionListRepository(IDbConnector dbConnector) => _dbConnector = dbConnector;

        private const string sql = @"SELECT ci.CatalogueItemId AS SolutionId, ci.[Name] AS SolutionName, sol.Summary AS SolutionSummary,
                sup.Id AS SupplierId, sup.[Name] AS SupplierName,
                cap.CapabilityRef AS CapabilityReference, cap.[Name] AS CapabilityName, cap.[Description] as CapabilityDescription,
                fs.IsFoundation AS IsFoundation
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
LEFT OUTER JOIN dbo.FrameworkSolutions AS fs
	         ON sol.Id = fs.SolutionId
		  WHERE ps.[Name] = 'Published'
		    AND ISNULL(fs.IsFoundation, 0) = COALESCE(@foundationOnly, fs.IsFoundation, 0)
			AND ci.SupplierId = ISNULL(@supplierId, ci.SupplierId);";

        /// <summary>
        /// Gets a list of <see cref="ISolutionListResult"/> objects.
        /// </summary>
        /// <returns>A list of <see cref="ISolutionListResult"/> objects.</returns>
        public async Task<IEnumerable<ISolutionListResult>> ListAsync(bool foundationOnly, string supplierId, CancellationToken cancellationToken)
        {
            supplierId = string.IsNullOrWhiteSpace(supplierId) ? null : supplierId;

            return await _dbConnector.QueryAsync<SolutionListResult>(
                    sql,
                    cancellationToken,
                    new { foundationOnly = foundationOnly ? (bool?)true : null, supplierId });
        }
    }
}
