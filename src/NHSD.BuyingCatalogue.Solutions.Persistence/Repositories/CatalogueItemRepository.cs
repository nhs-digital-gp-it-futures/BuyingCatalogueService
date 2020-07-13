using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence.CatalogueItems;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    public sealed class CatalogueItemRepository : ICatalogueItemRepository
    {
        private readonly IDbConnector _dbConnector;

        private const string DoesCatalogueItemExist = @"
            SELECT 1
            FROM dbo.CatalogueItem
            WHERE CatalogueItemId = @catalogueItemId;";
        
        private const string GetByIdSql = @"
            SELECT  ci.CatalogueItemId,
                    ci.[Name]
            FROM    dbo.CatalogueItem ci
            WHERE   ci.CatalogueItemId = @catalogueItemId;";


        public CatalogueItemRepository(IDbConnector dbConnector) =>
            _dbConnector = dbConnector ?? throw new ArgumentNullException(nameof(dbConnector));

        public async Task<bool> CheckExists(string catalogueItemId, CancellationToken cancellationToken)
        {
            var result = await _dbConnector.QueryAsync<int>(
                DoesCatalogueItemExist,
                cancellationToken,
                new {catalogueItemId});

            return result.Sum() == 1;
        }

        public async Task<ICatalogueItemResult> GetByIdAsync(string catalogueItemId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(catalogueItemId))
                return null;

            var result = await _dbConnector.QueryAsync<CatalogueItemResult>(
                GetByIdSql, 
                cancellationToken, 
                new { catalogueItemId });

            return result.SingleOrDefault();
        }
    }
}
