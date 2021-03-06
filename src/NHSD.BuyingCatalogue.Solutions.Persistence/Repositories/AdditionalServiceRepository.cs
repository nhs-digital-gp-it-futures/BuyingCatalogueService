﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Models;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Repositories
{
    public sealed class AdditionalServiceRepository : IAdditionalServiceRepository
    {
        private const string GetAdditionalService = @"
            SELECT ad.Summary,
                   ad.SolutionId,
                   ci.CatalogueItemId,
                   ci.[Name] AS CatalogueItemName,
                   solution.[Name] AS SolutionName
              FROM dbo.AdditionalService AS ad
                   INNER JOIN dbo.CatalogueItem AS ci on ad.CatalogueItemId = ci.CatalogueItemId
                   INNER JOIN dbo.CatalogueItem AS solution on ad.SolutionId = solution.CatalogueItemId
             WHERE ad.SolutionId in @solutionIds";

        private readonly IDbConnector dbConnector;

        public AdditionalServiceRepository(IDbConnector dbConnector)
        {
            this.dbConnector = dbConnector;
        }

        public async Task<IEnumerable<IAdditionalServiceResult>> GetAdditionalServiceBySolutionIdsAsync(
            IEnumerable<string> solutionIds,
            CancellationToken cancellationToken)
        {
            return await dbConnector.QueryAsync<AdditionalServiceResult>(
                GetAdditionalService,
                cancellationToken,
                new { solutionIds });
        }
    }
}
