﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence.CatalogueItems;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.CatalogueItems
{
    public sealed class CatalogueItemReader
    {
        private readonly ICatalogueItemRepository catalogueItemRepository;

        public CatalogueItemReader(ICatalogueItemRepository catalogueItemRepository)
        {
            this.catalogueItemRepository = catalogueItemRepository ?? throw new ArgumentNullException(nameof(catalogueItemRepository));
        }

        public async Task<CatalogueItemDto> GetByIdAsync(string catalogueItemId, CancellationToken cancellationToken)
        {
            var result = await catalogueItemRepository.GetByIdAsync(catalogueItemId, cancellationToken);
            if (result is null)
                throw new NotFoundException(nameof(CatalogueItemDto), catalogueItemId);

            return new CatalogueItemDto(result.CatalogueItemId, result.Name);
        }

        public async Task<IEnumerable<CatalogueItemDto>> ListAsync(
            string supplierId,
            CatalogueItemType? catalogueItemType,
            PublishedStatus? publishedStatus,
            CancellationToken cancellationToken)
        {
            var result = await catalogueItemRepository.ListAsync(
                supplierId,
                catalogueItemType,
                publishedStatus,
                cancellationToken);

            return result.Select(r => new CatalogueItemDto(r.CatalogueItemId, r.Name));
        }
    }
}
