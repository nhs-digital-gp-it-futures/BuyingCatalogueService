using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetPriceByPriceIdQuery : IRequest<ICataloguePrice>
    {
        public int PriceId { get; }

        public GetPriceByPriceIdQuery(int priceId)
        {
            PriceId = priceId;
        }
    }
}
