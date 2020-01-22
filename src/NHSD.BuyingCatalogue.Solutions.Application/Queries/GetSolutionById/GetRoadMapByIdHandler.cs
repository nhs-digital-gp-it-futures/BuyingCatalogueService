using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    internal sealed class GetRoadMapByIdHandler : IRequestHandler<GetRoadMapByIdQuery, string>
    {
        private readonly RoadMapReader _roadMapReader;
        private readonly SolutionVerifier _verifier;

        public GetRoadMapByIdHandler(RoadMapReader roadMapReader, SolutionVerifier verifier)
        {
            _roadMapReader = roadMapReader;
            _verifier = verifier;
        }

        public async Task<string> Handle(GetRoadMapByIdQuery request, CancellationToken cancellationToken)
        {
            await _verifier.ThrowWhenMissing(request.Id, cancellationToken).ConfigureAwait(false);
            return (await _roadMapReader.ByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)).Description;
        }
    }
}
