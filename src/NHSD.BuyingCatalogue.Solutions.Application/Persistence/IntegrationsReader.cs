using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class IntegrationsReader
    {
        private readonly ISolutionDetailRepository _solutionDetailRepository;

        public IntegrationsReader(ISolutionDetailRepository solutionDetailRepository)
        {
            _solutionDetailRepository = solutionDetailRepository;
        }

        public async Task<Integrations> BySolutionIdAsync(string id, CancellationToken cancellationToken)
        {
            var integrationsResult = await _solutionDetailRepository.GetIntegrationsBySolutionIdAsync(id, cancellationToken)
                .ConfigureAwait(false);
            return new Integrations{Url = integrationsResult.IntegrationsUrl};
        }
    }
}
