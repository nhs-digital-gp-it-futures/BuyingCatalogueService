using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence.CatalogueItems;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById
{
    public sealed class ListCatalogueItemHandler : IRequestHandler<ListCatalogueItemQuery, IEnumerable<CatalogueItemDto>>
    {
        private readonly CatalogueItemReader catalogueItemReader;

        public ListCatalogueItemHandler(CatalogueItemReader catalogueItemReader)
        {
            this.catalogueItemReader = catalogueItemReader ?? throw new ArgumentNullException(nameof(catalogueItemReader));
        }

        public async Task<IEnumerable<CatalogueItemDto>> Handle(ListCatalogueItemQuery request, CancellationToken cancellationToken)
        {
            return await catalogueItemReader.ListAsync(
                request?.SupplierId,
                request?.CatalogueItemType,
                request?.PublishedStatus,
                cancellationToken);
        }
    }
}
