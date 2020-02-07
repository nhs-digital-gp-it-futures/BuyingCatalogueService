using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetIntegrationsBySolutionId
{
    internal sealed class GetIntegrationsBySolutionIdHandler : IRequestHandler<GetIntegrationsBySolutionIdQuery, IIntegrations>
    {
        private readonly IntegrationsReader _integrationsReader;
        private readonly SolutionVerifier _verifier;
        private readonly IMapper _mapper;

        public GetIntegrationsBySolutionIdHandler(IntegrationsReader integrationsReader, SolutionVerifier verifier, IMapper mapper)
        {
            _integrationsReader = integrationsReader;
            _verifier = verifier;
            _mapper = mapper;
        }

        public async Task<IIntegrations> Handle(GetIntegrationsBySolutionIdQuery request, CancellationToken cancellationToken)
        {
            await _verifier.ThrowWhenMissingAsync(request.Id, cancellationToken).ConfigureAwait(false);
            var integrationsResult = (await _integrationsReader.BySolutionIdAsync(request.Id, cancellationToken).ConfigureAwait(false));
            return _mapper.Map<IIntegrations>(integrationsResult);
        }
    }
}
