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
        private readonly IntegrationsReader integrationsReader;
        private readonly SolutionVerifier verifier;
        private readonly IMapper mapper;

        public GetIntegrationsBySolutionIdHandler(IntegrationsReader integrationsReader, SolutionVerifier verifier, IMapper mapper)
        {
            this.integrationsReader = integrationsReader;
            this.verifier = verifier;
            this.mapper = mapper;
        }

        public async Task<IIntegrations> Handle(GetIntegrationsBySolutionIdQuery request, CancellationToken cancellationToken)
        {
            await verifier.ThrowWhenMissingAsync(request.Id, cancellationToken);
            var integrationsResult = await integrationsReader.BySolutionIdAsync(request.Id, cancellationToken);
            return mapper.Map<IIntegrations>(integrationsResult);
        }
    }
}
