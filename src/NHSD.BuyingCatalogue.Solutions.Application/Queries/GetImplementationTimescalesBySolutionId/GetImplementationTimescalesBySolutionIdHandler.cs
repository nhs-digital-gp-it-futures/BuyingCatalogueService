using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetImplementationTimescalesBySolutionId
{
    internal sealed class GetImplementationTimescalesBySolutionIdHandler : IRequestHandler<GetImplementationTimescalesBySolutionIdQuery, IImplementationTimescales>
    {
        private readonly ImplementationTimescalesReader _implementationTimescalesReader;
        private readonly SolutionVerifier _verifier;
        private readonly IMapper _mapper;

        public GetImplementationTimescalesBySolutionIdHandler(ImplementationTimescalesReader implementationTimescalesReader, SolutionVerifier verifier, IMapper mapper)
        {
            _implementationTimescalesReader = implementationTimescalesReader;
            _verifier = verifier;
            _mapper = mapper;
        }

        public async Task<IImplementationTimescales> Handle(GetImplementationTimescalesBySolutionIdQuery request, CancellationToken cancellationToken)
        {
            await _verifier.ThrowWhenMissingAsync(request.Id, cancellationToken).ConfigureAwait(false);
            var implementationTimescalesResult = (await _implementationTimescalesReader.BySolutionIdAsync(request.Id, cancellationToken).ConfigureAwait(false));
            return _mapper.Map<IImplementationTimescales>(implementationTimescalesResult);
        }
    }
}
