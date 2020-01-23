using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetRoadMapBySolutionId
{
    internal sealed class GetRoadMapBySolutionIdHandler : IRequestHandler<GetRoadMapBySolutionIdQuery, IRoadMap>
    {
        private readonly RoadMapReader _roadMapReader;
        private readonly SolutionVerifier _verifier;
        private readonly IMapper _mapper;

        public GetRoadMapBySolutionIdHandler(RoadMapReader roadMapReader, SolutionVerifier verifier, IMapper mapper)
        {
            _roadMapReader = roadMapReader;
            _verifier = verifier;
            _mapper = mapper;
        }

        public async Task<IRoadMap> Handle(GetRoadMapBySolutionIdQuery request, CancellationToken cancellationToken)
        {
            await _verifier.ThrowWhenMissing(request.Id, cancellationToken).ConfigureAwait(false);
            var roadMapResult = (await _roadMapReader.BySolutionIdAsync(request.Id, cancellationToken).ConfigureAwait(false));
            return _mapper.Map<IRoadMap>(roadMapResult);
        }
    }
}
