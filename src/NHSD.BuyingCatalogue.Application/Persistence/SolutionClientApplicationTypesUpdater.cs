using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class SolutionClientApplicationTypesUpdater
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly IMarketingDetailRepository _marketingDetailRepository;

        public SolutionClientApplicationTypesUpdater(IMarketingDetailRepository marketingDetailRepository)
        {
            _marketingDetailRepository = marketingDetailRepository;
        }

        public async Task UpdateAsync(ClientApplication clientApplication, string solutionId, CancellationToken cancellationToken)
        {
            await _marketingDetailRepository.UpdateClientApplicationAsync(new UpdateSolutionClientApplicationRequest
            {
                Id = solutionId,
                ClientApplication = JsonConvert.SerializeObject(clientApplication).ToString()
            }, cancellationToken);
        }
    }
}
