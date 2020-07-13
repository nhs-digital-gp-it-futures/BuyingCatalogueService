using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    public sealed class PriceRepository : IPriceRepository
    {
        private readonly IDbConnector _dbConnector;

        private const string ListCataloguePricesSql = @"
        SELECT  cp.CatalogueItemId,
                ci.[Name] AS CatalogueItemName,
                cp.CataloguePriceId,
                cp.CataloguePriceTypeId,
                cp.ProvisioningTypeId,
                pu.PricingUnitId,
                pu.[Name] AS PricingUnitName,
                pu.[Description] AS PricingUnitDescription,
                pu.TierName AS PricingUnitTierName,
                cp.TimeUnitId,
                cp.CurrencyCode,
                cp.Price AS FlatPrice,
                cptr.BandStart,
                cptr.BandEnd,
                cptr.Price AS TieredPrice
        FROM    dbo.CataloguePrice AS cp
                INNER JOIN dbo.PricingUnit AS pu ON pu.PricingUnitId = cp.PricingUnitId
                INNER JOIN dbo.CatalogueItem AS ci ON ci.CatalogueItemId = cp.CatalogueItemId
                LEFT OUTER JOIN dbo.CataloguePriceTier AS cptr ON cptr.CataloguePriceId = cp.CataloguePriceId
        WHERE   cp.CatalogueItemId = @catalogueItemId;";

        private const string CataloguePriceSql = @"
        SELECT  cp.CatalogueItemId,
                cp.CataloguePriceId,
                cp.CataloguePriceTypeId,
                cp.ProvisioningTypeId,
                pu.PricingUnitId,
                pu.[Name] AS PricingUnitName,
                pu.[Description] as PricingUnitDescription,
                pu.TierName as PricingUnitTierName,
                cp.TimeUnitId,
                cp.CurrencyCode,
                cp.Price AS FlatPrice,
                cptr.BandStart,
                cptr.BandEnd,
                cptr.Price AS TieredPrice
        FROM    dbo.CataloguePrice As cp
                INNER JOIN dbo.PricingUnit AS pu ON pu.PricingUnitId = cp.PricingUnitId
                LEFT OUTER JOIN dbo.CataloguePriceTier AS cptr ON cptr.CataloguePriceId = cp.CataloguePriceId
        WHERE   cp.CataloguePriceId = @cataloguePriceId;";

        public PriceRepository(IDbConnector dbConnector)
        {
            _dbConnector = dbConnector;
        }

        public async Task<IEnumerable<ICataloguePriceListResult>> GetPricesBySolutionIdQueryAsync(string solutionId, CancellationToken cancellationToken)
        {
            return await _dbConnector.QueryAsync<CataloguePriceListResult>(ListCataloguePricesSql, cancellationToken,
                new { catalogueItemId = solutionId });
        }

        public async Task<IEnumerable<ICataloguePriceListResult>> GetPricesByCatalogueItemIdQueryAsync(string catalogueItemId, CancellationToken cancellationToken)
        {
            return await _dbConnector.QueryAsync<CataloguePriceListResult>(ListCataloguePricesSql, cancellationToken,
                new { catalogueItemId });
        }

        public async Task<IEnumerable<ICataloguePriceListResult>> GetPriceByPriceIdQueryAsync(int cataloguePriceId, CancellationToken cancellationToken)
        {
            return (await _dbConnector.QueryAsync<CataloguePriceListResult>(CataloguePriceSql, cancellationToken,
                new { cataloguePriceId }));
        }
    }
}
