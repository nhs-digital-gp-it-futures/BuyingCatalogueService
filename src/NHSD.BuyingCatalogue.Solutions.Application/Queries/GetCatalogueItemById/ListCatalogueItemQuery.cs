using System.Collections.Generic;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById
{
    public sealed class ListCatalogueItemQuery : IRequest<IEnumerable<CatalogueItemDto>>
    {
        public ListCatalogueItemQuery(
            string supplierId,
            CatalogueItemType? catalogueItemType,
            PublishedStatus? publishedStatus)
        {
            SupplierId = supplierId;
            CatalogueItemType = catalogueItemType;
            PublishedStatus = publishedStatus;
        }

        public string SupplierId { get; }

        public CatalogueItemType? CatalogueItemType { get; }

        public PublishedStatus? PublishedStatus { get; }
    }
}
