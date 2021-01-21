using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetPriceByPriceIdQuery : IRequest<ICataloguePrice>
    {
        public GetPriceByPriceIdQuery(int priceId)
        {
            PriceId = priceId;
        }

        public int PriceId { get; }
    }
}
