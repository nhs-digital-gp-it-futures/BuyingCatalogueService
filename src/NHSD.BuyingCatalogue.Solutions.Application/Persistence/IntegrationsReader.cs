using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class IntegrationsReader
    {
        private readonly ISolutionDetailRepository solutionDetailRepository;

        public IntegrationsReader(ISolutionDetailRepository solutionDetailRepository)
        {
            this.solutionDetailRepository = solutionDetailRepository;
        }

        public async Task<Integrations> BySolutionIdAsync(string id, CancellationToken cancellationToken)
        {
            var integrationsResult = await solutionDetailRepository.GetIntegrationsBySolutionIdAsync(id, cancellationToken);
            return new Integrations { Url = integrationsResult.IntegrationsUrl };
        }
    }
}
