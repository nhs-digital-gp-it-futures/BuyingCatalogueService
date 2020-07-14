﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence.CatalogueItems;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    public sealed class CatalogueItemRepository : ICatalogueItemRepository
    {
        private readonly IDbConnector _dbConnector;

        private const string GetByIdSql = @"
        SELECT  ci.CatalogueItemId,
                ci.[Name]
        FROM    dbo.CatalogueItem ci
        WHERE   ci.CatalogueItemId = @catalogueItemId;";

        private const string ListSql = @"
        SELECT  ci.CatalogueItemId,
                ci.[Name]
        FROM    dbo.CatalogueItem ci
        WHERE   ci.SupplierId = ISNULL(NULLIF(@supplierId, ''), ci.SupplierId)
        AND     ci.CatalogueItemTypeId = ISNULL(NULLIF(@catalogueItemType, ''), ci.CatalogueItemTypeId)";

        public CatalogueItemRepository(IDbConnector dbConnector) =>
            _dbConnector = dbConnector ?? throw new ArgumentNullException(nameof(dbConnector));

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

        public async Task<IEnumerable<ICatalogueItemResult>> ListAsync(string supplierId,
            CatalogueItemType? catalogueItemType, CancellationToken cancellationToken)
        {
            return await _dbConnector.QueryAsync<CatalogueItemResult>(
                ListSql,
                cancellationToken,
                new { supplierId, catalogueItemType });
        }
    }
}