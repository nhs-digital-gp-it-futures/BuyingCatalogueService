using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class ClientApplicationReader
    {
        private readonly ISolutionRepository solutionRepository;

        public ClientApplicationReader(ISolutionRepository solutionRepository)
        {
            this.solutionRepository = solutionRepository;
        }

        public async Task<ClientApplication> BySolutionIdAsync(string id, CancellationToken cancellationToken)
        {
            var clientApplicationResult = await solutionRepository.GetClientApplicationBySolutionIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(nameof(Solution), id);

            return string.IsNullOrWhiteSpace(clientApplicationResult.ClientApplication)
                ? new ClientApplication()
                : JsonConvert.DeserializeObject<ClientApplication>(clientApplicationResult.ClientApplication);
        }
    }
}
