using System.Collections.Generic;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById
{
    public sealed class ListCatalogueItemQuery : IRequest<IEnumerable<CatalogueItemDto>>
    {
        public string SupplierId { get; }

        public CatalogueItemType? CatalogueItemType { get; }

        public ListCatalogueItemQuery(string supplierId, CatalogueItemType? catalogueItemType)
        {
            SupplierId = supplierId;
            CatalogueItemType = catalogueItemType;
        }
    }
}
