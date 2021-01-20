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
        private readonly AdditionalServiceReader additionalServiceReader;
        private readonly IMapper mapper;

        public GetAdditionalServiceByCatalogueItemIdHandler(AdditionalServiceReader additionalServiceReader, IMapper mapper)
        {
            this.additionalServiceReader = additionalServiceReader;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<IAdditionalService>> Handle(GetAdditionalServiceBySolutionIdsQuery request, CancellationToken cancellationToken)
        {
            var result = await additionalServiceReader.GetAdditionalServiceByAdditionalServiceIdAsync(
                request.SolutionIds,
                cancellationToken);

            return mapper.Map<IEnumerable<IAdditionalService>>(result);
        }
    }
}
