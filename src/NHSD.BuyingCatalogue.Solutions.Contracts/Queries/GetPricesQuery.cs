using System.Collections.Generic;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetPricesQuery : IRequest<IEnumerable<ICataloguePrice>>
    {
        public GetPricesQuery(string catalogueItemId)
        {
            CatalogueItemId = catalogueItemId;
        }

        public string CatalogueItemId { get; }
    }
}
