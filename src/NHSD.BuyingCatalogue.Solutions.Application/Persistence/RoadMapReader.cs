using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class RoadMapReader
    {
        private readonly ISolutionDetailRepository _solutionDetailRepository;

        public RoadMapReader(ISolutionDetailRepository solutionDetailRepository)
        {
            _solutionDetailRepository = solutionDetailRepository;
        }

        public async Task<IRoadMapResult> BySolutionIdAsync(string id, CancellationToken cancellationToken)
        {
          return await _solutionDetailRepository.GetRoadMapBySolutionIdAsync(id, cancellationToken).ConfigureAwait(false);
        }
    }
}
