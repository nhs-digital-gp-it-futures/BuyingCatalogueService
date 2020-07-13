using System.Collections.Generic;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetPriceByCatalogueItemIdQuery : IRequest<IEnumerable<ICataloguePrice>>
    {
        public string CatalogueItemId { get; }

        public GetPriceByCatalogueItemIdQuery(string catalogueItemId)
        {
            CatalogueItemId = catalogueItemId;
        }
    }
}
