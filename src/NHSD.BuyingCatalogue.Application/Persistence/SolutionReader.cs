using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class SolutionReader
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionRepository _solutionRepository;

        public SolutionReader(ISolutionRepository solutionRepository) => _solutionRepository = solutionRepository;

        public async Task<Solution> ByIdAsync(string id, CancellationToken cancellationToken) =>
            new Solution((await _solutionRepository.ByIdAsync(id, cancellationToken))
                         ?? throw new NotFoundException(nameof(Solution), id));
    }
}
