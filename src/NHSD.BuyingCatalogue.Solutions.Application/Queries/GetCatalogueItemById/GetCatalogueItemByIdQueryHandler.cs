using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence.CatalogueItems;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById
{
    public sealed class GetCatalogueItemByIdQueryHandler : IRequestHandler<GetCatalogueItemByIdQuery, CatalogueItemDto>
    {
        private readonly CatalogueItemReader _catalogueItemReader;

        public GetCatalogueItemByIdQueryHandler(CatalogueItemReader catalogueItemReader)
        {
            _catalogueItemReader = catalogueItemReader ?? throw new ArgumentNullException(nameof(catalogueItemReader));
        }

        public async Task<CatalogueItemDto> Handle(GetCatalogueItemByIdQuery request, CancellationToken cancellationToken) => 
            await _catalogueItemReader.GetByIdAsync(request?.CatalogueItemId, cancellationToken);
    }
}
