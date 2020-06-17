using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    public sealed class PricingRepository : IPricingRepository
    {
        private readonly IDbConnector _dbConnector;

        private const string ListCataloguePricesSql = @"
        SELECT	cp.CatalogueItemId,
                cp.CataloguePriceId,
                cp.ProvisioningTypeId,
                cp.CataloguePriceTypeId,
                pu.PricingUnitId,
                pu.[Name],
                pu.[Description],
                cp.TimeUnitId,
                cp.CurrencyCode,
                cpf.Price AS FlatPrice,
                cptr.BandStart,
                cptr.BandEnd,
                cptr.Price AS TieredPrice
        FROM	CataloguePrice cp
                INNER JOIN PricingUnit pu ON pu.PricingUnitId = cp.PricingUnitId
                LEFT OUTER JOIN CataloguePriceFlat cpf ON cpf.CataloguePriceId = cp.CataloguePriceId
                LEFT OUTER JOIN CataloguePriceTier cptr ON cptr.CataloguePriceId = cp.CataloguePriceId
        WHERE   cp.CatalogueItemId = @solutionId;";

        public PricingRepository(IDbConnector dbConnector) => _dbConnector = dbConnector;

        public async Task<IEnumerable<ICataloguePriceListResult>> GetPricingBySolutionIdQuery(string solutionId, CancellationToken cancellationToken)
        {
            return (await _dbConnector.QueryAsync<CataloguePriceListResult>(ListCataloguePricesSql, cancellationToken,
                new {solutionId}));
        }
    }
}
