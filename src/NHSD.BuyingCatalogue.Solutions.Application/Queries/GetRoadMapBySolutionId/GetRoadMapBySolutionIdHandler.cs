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
        private readonly RoadMapReader roadMapReader;
        private readonly SolutionVerifier verifier;
        private readonly IMapper mapper;

        public GetRoadMapBySolutionIdHandler(RoadMapReader roadMapReader, SolutionVerifier verifier, IMapper mapper)
        {
            this.roadMapReader = roadMapReader;
            this.verifier = verifier;
            this.mapper = mapper;
        }

        public async Task<IRoadMap> Handle(GetRoadMapBySolutionIdQuery request, CancellationToken cancellationToken)
        {
            await verifier.ThrowWhenMissingAsync(request.Id, cancellationToken);
            var roadMapResult = await roadMapReader.BySolutionIdAsync(request.Id, cancellationToken);
            return mapper.Map<IRoadMap>(roadMapResult);
        }
    }
}
