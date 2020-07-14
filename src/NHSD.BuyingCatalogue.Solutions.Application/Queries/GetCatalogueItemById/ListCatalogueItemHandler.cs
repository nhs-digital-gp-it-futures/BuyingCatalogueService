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
        private readonly CatalogueItemReader _catalogueItemReader;

        public ListCatalogueItemHandler(CatalogueItemReader catalogueItemReader)
        {
            _catalogueItemReader = catalogueItemReader ?? throw new ArgumentNullException(nameof(catalogueItemReader));
        }

        public async Task<IEnumerable<CatalogueItemDto>> Handle(ListCatalogueItemQuery request, CancellationToken cancellationToken)
        {
            return await _catalogueItemReader.ListAsync(request?.SupplierId, request?.CatalogueItemType, cancellationToken);
        }
    }
}
