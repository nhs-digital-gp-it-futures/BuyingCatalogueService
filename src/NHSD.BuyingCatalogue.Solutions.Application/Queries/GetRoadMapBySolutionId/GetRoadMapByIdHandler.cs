using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetRoadMapBySolutionId
{
    internal sealed class GetRoadMapByIdHandler : IRequestHandler<GetRoadMapBySolutionIdQuery, IRoadMap>
    {
        private readonly RoadMapReader _roadMapReader;
        private readonly SolutionVerifier _verifier;

        public GetRoadMapByIdHandler(RoadMapReader roadMapReader, SolutionVerifier verifier)
        {
            _roadMapReader = roadMapReader;
            _verifier = verifier;
        }

        public async Task<IRoadMap> Handle(GetRoadMapBySolutionIdQuery request, CancellationToken cancellationToken)
        {
            await _verifier.ThrowWhenMissing(request.Id, cancellationToken).ConfigureAwait(false);
            var roadMapResult = (await _roadMapReader.BySolutionIdAsync(request.Id, cancellationToken).ConfigureAwait(false));
            return new RoadMapDto { Summary = roadMapResult?.Summary };

        }
    }
}
