using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class AdditionalServiceReader
    {
        private readonly IAdditionalServiceRepository _additionalServiceRepository;

        public AdditionalServiceReader(IAdditionalServiceRepository additionalServiceRepository)
        {
            _additionalServiceRepository = additionalServiceRepository;
        }

        public async Task<IEnumerable<AdditionalService>> GetAdditionalServiceByAdditionalServiceIdAsync(IEnumerable<string> solutionIds,
            CancellationToken cancellationToken)
        {
            var additionalService = await 
                _additionalServiceRepository.GetAdditionalServiceBySolutionIdsAsync(solutionIds,
                    cancellationToken);

            return additionalService.Select(additionalServiceResult => new AdditionalService(additionalServiceResult));
        }
    }
}
