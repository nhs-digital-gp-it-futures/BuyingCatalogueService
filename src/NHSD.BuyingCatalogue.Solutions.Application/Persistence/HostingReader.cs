using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class HostingReader
    {
        private readonly ISolutionRepository solutionRepository;

        public HostingReader(ISolutionRepository solutionRepository)
        {
            this.solutionRepository = solutionRepository;
        }

        public async Task<Hosting> BySolutionIdAsync(string id, CancellationToken cancellationToken)
        {
            var hostingResult = await solutionRepository.GetHostingBySolutionIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(nameof(Solution), id);

            return string.IsNullOrWhiteSpace(hostingResult.Hosting)
                ? new Hosting()
                : JsonConvert.DeserializeObject<Hosting>(hostingResult.Hosting);
        }
    }
}
