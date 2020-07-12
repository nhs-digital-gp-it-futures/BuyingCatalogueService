using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById
{
    public sealed class GetCatalogueItemByIdQuery : IRequest<CatalogueItemDto>
    {
        public string CatalogueItemId { get; }

        public GetCatalogueItemByIdQuery(string catalogueItemId)
        {
            CatalogueItemId = catalogueItemId;
        }
    }
}
