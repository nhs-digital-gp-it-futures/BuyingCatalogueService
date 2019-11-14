using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class SolutionFeaturesUpdater
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly IMarketingDetailRepository _marketingDetailRepository;

        public SolutionFeaturesUpdater(IMarketingDetailRepository marketingDetailRepository)
        {
            _marketingDetailRepository = marketingDetailRepository;
        }

        public async Task UpdateAsync(Solution solution, CancellationToken cancellationToken)
        {
            await _marketingDetailRepository.UpdateFeaturesAsync(Map(solution), cancellationToken);
        }

        private IUpdateSolutionFeaturesRequest Map(Solution solution)
            => new UpdateSolutionFeaturesRequest
            {
                Id = solution.Id,
                Features = JsonConvert.SerializeObject(solution.Features).ToString()
            };
    }
}
