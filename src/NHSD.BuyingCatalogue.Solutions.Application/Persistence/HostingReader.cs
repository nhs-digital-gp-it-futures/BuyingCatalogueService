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
        private readonly ISolutionDetailRepository solutionDetailRepository;

        public HostingReader(ISolutionDetailRepository solutionDetailRepository)
        {
            this.solutionDetailRepository = solutionDetailRepository;
        }

        public async Task<Hosting> BySolutionIdAsync(string id, CancellationToken cancellationToken)
        {
            var hostingResult = await solutionDetailRepository.GetHostingBySolutionIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(nameof(Solution), id);

            return string.IsNullOrWhiteSpace(hostingResult.Hosting)
                ? new Hosting()
                : JsonConvert.DeserializeObject<Hosting>(hostingResult.Hosting);
        }
    }
}
