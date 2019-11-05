using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
using NHSD.BuyingCatalogue.Contracts;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class SolutionClientApplicationUpdater
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly IMarketingDetailRepository _marketingDetailRepository;

        public SolutionClientApplicationUpdater(IMarketingDetailRepository marketingDetailRepository)
        {
            _marketingDetailRepository = marketingDetailRepository;
        }

        public async Task UpdateAsync(IClientApplication clientApplication, string solutionId, CancellationToken cancellationToken)
        {
            await _marketingDetailRepository.UpdateClientApplicationAsync(new UpdateSolutionClientApplicationRequest
            {
                Id = solutionId,
                ClientApplication = JsonConvert.SerializeObject(clientApplication).ToString()
            }, cancellationToken);
        }
    }
}
