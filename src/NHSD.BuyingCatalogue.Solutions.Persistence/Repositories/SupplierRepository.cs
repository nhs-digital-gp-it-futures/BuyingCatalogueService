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
    public sealed class SupplierRepository : ISupplierRepository
    {
        private const string GetSupplierBySolutionIdSql = @"SELECT
                                    Solution.Id as SolutionId,
                                    Supplier.Name as Name,
                                    Supplier.Summary as Summary,
                                    Supplier.SupplierUrl as Url
                                 FROM Supplier
                                      LEFT JOIN Solution ON Supplier.Id = Solution.SupplierId
                                 WHERE  Solution.Id = @solutionId";

        private const string UpdateSupplierBySolutionIdSql = @"UPDATE
                                    Supplier
                                    SET
                                    Supplier.Summary = @Summary,
                                    Supplier.SupplierUrl = @SupplierUrl,
                                    Supplier.LastUpdated = GETDATE()
                                    FROM Supplier
                                        INNER JOIN Solution
                                        ON Supplier.Id = Solution.SupplierId
                                        WHERE Solution.Id = @solutionId";

        // A full-text index is typically a better way of implementing this kind of search. For example, the LIKE search below will
        // always perform an index scan. Given the expected number of suppliers I doubt this will be a problem, however.
        private const string GetSuppliersByNameSql = @"SELECT Id, [Name] FROM dbo.Supplier WHERE [Name] LIKE '%' + @name + '%' ORDER BY [Name];";

        private readonly IDbConnector _dbConnector;

        public SupplierRepository(IDbConnector dbConnector) => _dbConnector = dbConnector;

        public async Task<ISupplierResult> GetSupplierBySolutionIdAsync(string solutionId, CancellationToken cancellationToken) =>
            (await _dbConnector
                .QueryAsync<SupplierResult>(GetSupplierBySolutionIdSql, cancellationToken, new { solutionId })
                .ConfigureAwait(false)).SingleOrDefault();

        public async Task<IEnumerable<ISupplierNameResult>> GetSuppliersByName(string name, CancellationToken cancellationToken) =>
            await _dbConnector.QueryAsync<SupplierNameResult>(GetSuppliersByNameSql, cancellationToken, new { Name = name ?? string.Empty });

        public async Task UpdateSupplierAsync(IUpdateSupplierRequest updateSupplierRequest, CancellationToken cancellationToken)
        {
            if (updateSupplierRequest == null)
            {
                throw new ArgumentNullException(nameof(updateSupplierRequest));
            }

            await _dbConnector.ExecuteAsync(UpdateSupplierBySolutionIdSql, cancellationToken,
                new
                {
                    solutionId = updateSupplierRequest.SolutionId,
                    summary = updateSupplierRequest.Description,
                    supplierUrl = updateSupplierRequest.Link
                }).ConfigureAwait(false);
        }
    }
}
