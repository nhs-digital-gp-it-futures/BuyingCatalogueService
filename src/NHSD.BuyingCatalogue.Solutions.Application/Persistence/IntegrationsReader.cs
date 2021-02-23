using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class IntegrationsReader
    {
        private readonly ISolutionRepository solutionRepository;

        public IntegrationsReader(ISolutionRepository solutionRepository)
        {
            this.solutionRepository = solutionRepository;
        }

        public async Task<Integrations> BySolutionIdAsync(string id, CancellationToken cancellationToken)
        {
            var integrationsResult = await solutionRepository.GetIntegrationsBySolutionIdAsync(id, cancellationToken);
            return new Integrations { Url = integrationsResult.IntegrationsUrl };
        }
    }
}
