using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId
{
    internal sealed class GetPricingBySolutionIdHandler : IRequestHandler<GetPricingBySolutionIdQuery, IPricingUnit>
    {
        private readonly PricingReader _pricingReader;
        private readonly IMapper _mapper;

        public GetPricingBySolutionIdHandler(PricingReader pricingReader, IMapper mapper)
        {
            _pricingReader = pricingReader;
            _mapper = mapper;
        }

        public async Task<IPricingUnit> Handle(GetPricingBySolutionIdQuery request, CancellationToken cancellationToken) => 
            _mapper.Map<IPricingUnit>(await _pricingReader.GetBySolutionIdAsync(request.SolutionId, cancellationToken));
    }
}
