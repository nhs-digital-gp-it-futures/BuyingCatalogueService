using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetAdditionalServiceByAdditionalServiceId
{
    internal sealed class GetAdditionalServiceByCatalogueItemIdHandler : IRequestHandler<GetAdditionalServiceBySolutionIdsQuery, IEnumerable<IAdditionalService>>
    {
        private readonly AdditionalServiceReader _additionalServiceReader;
        private readonly IMapper _mapper;

        public GetAdditionalServiceByCatalogueItemIdHandler(AdditionalServiceReader additionalServiceReader, IMapper mapper)
        {
            _additionalServiceReader = additionalServiceReader;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IAdditionalService>> Handle(GetAdditionalServiceBySolutionIdsQuery request, CancellationToken cancellationToken)
        {
            var result =
                await _additionalServiceReader.GetAdditionalServiceByAdditionalServiceIdAsync(request.SolutionIds,
                    cancellationToken);

            return _mapper.Map<IEnumerable<IAdditionalService>>(result);
        }
    }
}
