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
        private readonly ImplementationTimescalesReader implementationTimescalesReader;
        private readonly SolutionVerifier verifier;
        private readonly IMapper mapper;

        public GetImplementationTimescalesBySolutionIdHandler(
            ImplementationTimescalesReader implementationTimescalesReader,
            SolutionVerifier verifier,
            IMapper mapper)
        {
            this.implementationTimescalesReader = implementationTimescalesReader;
            this.verifier = verifier;
            this.mapper = mapper;
        }

        public async Task<IImplementationTimescales> Handle(
            GetImplementationTimescalesBySolutionIdQuery request,
            CancellationToken cancellationToken)
        {
            await verifier.ThrowWhenMissingAsync(request.Id, cancellationToken);
            var implementationTimescalesResult = await implementationTimescalesReader.BySolutionIdAsync(
                request.Id,
                cancellationToken);

            return mapper.Map<IImplementationTimescales>(implementationTimescalesResult);
        }
    }
}
