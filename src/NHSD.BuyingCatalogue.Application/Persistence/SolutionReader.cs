using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class SolutionReader
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionRepository _solutionRepository;

        public SolutionReader(ISolutionRepository solutionRepository)
        {
            _solutionRepository = solutionRepository;
        }

        public async Task<Solution> ByIdAsync(string id, CancellationToken cancellationToken)
        {
            var solution = (await _solutionRepository.ByIdAsync(id, cancellationToken))
                ?? throw new NotFoundException(nameof(Solution), id);

            return Map(solution);
        }

        private Solution Map(ISolutionResult solutionResult)
            => new Solution
            {
                Id = solutionResult.Id,
                Name = solutionResult.Name,
                Summary = solutionResult.Summary,
                Description = solutionResult.Description,
                Features = solutionResult.Features,
                AboutUrl = solutionResult.AboutUrl,
                ClientApplication = solutionResult.ClientApplication == null ? null : JsonConvert.DeserializeObject<ClientApplication>(solutionResult.ClientApplication)
            };
    }
}

