using System.Collections.Generic;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetSuppliersByNameQuery : IRequest<IEnumerable<ISupplier>>
    {
        public GetSuppliersByNameQuery(
            string name,
            PublishedStatus? solutionPublicationStatus,
            CatalogueItemType? catalogueItemType)
        {
            Name = name;
            SolutionPublicationStatus = solutionPublicationStatus;
            CatalogueItemType = catalogueItemType;
        }

        public string Name { get; }

        public PublishedStatus? SolutionPublicationStatus { get; }

        public CatalogueItemType? CatalogueItemType { get; }
    }
}
