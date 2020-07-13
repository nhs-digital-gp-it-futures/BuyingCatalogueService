using System.Collections.Generic;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetPricesQuery : IRequest<IEnumerable<ICataloguePrice>>
    {
        public string CatalogueItemId { get; }

        public GetPricesQuery(string catalogueItemId)
        {
            CatalogueItemId = catalogueItemId;
        }
    }
}
