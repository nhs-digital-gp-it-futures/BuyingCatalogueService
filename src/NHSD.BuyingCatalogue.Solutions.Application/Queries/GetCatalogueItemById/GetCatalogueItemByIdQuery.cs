using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById
{
    public sealed class GetCatalogueItemByIdQuery : IRequest<CatalogueItemDto>
    {
        public GetCatalogueItemByIdQuery(string catalogueItemId)
        {
            CatalogueItemId = catalogueItemId;
        }

        public string CatalogueItemId { get; }
    }
}
