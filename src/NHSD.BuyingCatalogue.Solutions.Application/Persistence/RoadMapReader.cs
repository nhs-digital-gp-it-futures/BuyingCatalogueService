using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
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

        public async Task<RoadMap> BySolutionIdAsync(string id, CancellationToken cancellationToken)
        {
            var roadMapResult = await _solutionDetailRepository.GetRoadMapBySolutionIdAsync(id, cancellationToken)
                .ConfigureAwait(false);
          return new RoadMap{Summary = roadMapResult.Summary};
        }
    }
}
