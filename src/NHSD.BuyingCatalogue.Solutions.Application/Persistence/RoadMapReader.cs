using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class RoadMapReader
    {
        private readonly ISolutionDetailRepository solutionDetailRepository;

        public RoadMapReader(ISolutionDetailRepository solutionDetailRepository)
        {
            this.solutionDetailRepository = solutionDetailRepository;
        }

        public async Task<RoadMap> BySolutionIdAsync(string id, CancellationToken cancellationToken)
        {
            var roadMapResult = await solutionDetailRepository.GetRoadMapBySolutionIdAsync(id, cancellationToken);
            return new RoadMap { Summary = roadMapResult.Summary };
        }
    }
}
