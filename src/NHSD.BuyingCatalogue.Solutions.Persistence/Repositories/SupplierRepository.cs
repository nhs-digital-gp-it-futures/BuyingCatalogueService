using System;
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
        private readonly IDbConnector _dbConnector;

        public SupplierRepository(IDbConnector dbConnector) => _dbConnector = dbConnector;

        private const string getSupplierBySolutionIdSql = @"SELECT
                                    Solution.Id as SolutionId,
                                    Supplier.Name as Name,
                                    Supplier.Summary as Summary,
                                    Supplier.SupplierUrl as Url
                                 FROM Supplier
                                      LEFT JOIN Solution ON Supplier.Id = Solution.SupplierId
                                 WHERE  Solution.Id = @solutionId";

        private const string updateSupplierBySolutionId = @"UPDATE
                                    Supplier
                                    SET
                                    Supplier.Summary = @Summary,
                                    Supplier.SupplierUrl = @SupplierUrl,
                                    Supplier.LastUpdated = GETDATE()
                                    FROM Supplier
                                        INNER JOIN Solution
                                        ON Supplier.Id = Solution.SupplierId
                                        WHERE Solution.Id = @solutionId";


        public async Task<ISupplierResult> GetSupplierBySolutionIdAsync(string solutionId,
            CancellationToken cancellationToken) =>
            (await _dbConnector
                .QueryAsync<SupplierResult>(getSupplierBySolutionIdSql, cancellationToken, new {solutionId})
                .ConfigureAwait(false)).SingleOrDefault();

        public async Task UpdateSupplierAsync(IUpdateSupplierRequest updateSupplierRequest, CancellationToken cancellationToken)
        {
            if (updateSupplierRequest == null)
            {
                throw new ArgumentNullException(nameof(updateSupplierRequest));
            }

            await _dbConnector.ExecuteAsync(updateSupplierBySolutionId, cancellationToken,
                new
                {
                    solutionId = updateSupplierRequest.SolutionId,
                    summary = updateSupplierRequest?.Description,
                    supplierUrl = updateSupplierRequest?.Link
                }).ConfigureAwait(false);
        }
    }
}
