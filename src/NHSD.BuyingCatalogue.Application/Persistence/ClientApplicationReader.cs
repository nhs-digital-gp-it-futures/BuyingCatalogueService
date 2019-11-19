using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class ClientApplicationReader
    {
        private readonly ISolutionDetailRepository _solutionDetailRepository;

        public ClientApplicationReader(ISolutionDetailRepository solutionDetailRepository)
        {
            _solutionDetailRepository = solutionDetailRepository;
        }

        public async Task<ClientApplication> BySolutionIdAsync(string id, CancellationToken cancellationToken)
        {
            var clientApplicationResult = await _solutionDetailRepository.GetClientApplicationBySolutionIdAsync(id, cancellationToken)
                                 ?? throw new NotFoundException(nameof(Solution), id);

            return string.IsNullOrWhiteSpace(clientApplicationResult.ClientApplication)
                ? new ClientApplication()
                : JsonConvert.DeserializeObject<ClientApplication>(clientApplicationResult.ClientApplication);
        }
    }
}
