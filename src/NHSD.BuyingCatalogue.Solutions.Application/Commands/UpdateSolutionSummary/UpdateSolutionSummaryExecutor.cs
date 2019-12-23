using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary
{
    internal sealed class UpdateSolutionSummaryExecutor : IExecutor<UpdateSolutionSummaryCommand>
    {
        private readonly SolutionReader _solutionReader;
        private readonly SolutionSummaryUpdater _solutionSummaryUpdater;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionHandler"/> class.
        /// </summary>
        public UpdateSolutionSummaryExecutor(SolutionReader solutionReader,
            SolutionSummaryUpdater solutionSummaryUpdater,
            IMapper mapper)
        {
            _solutionReader = solutionReader;
            _solutionSummaryUpdater = solutionSummaryUpdater;
            _mapper = mapper;
        }

        public async Task UpdateAsync(UpdateSolutionSummaryCommand request, CancellationToken cancellationToken) =>
            await _solutionSummaryUpdater.UpdateSummaryAsync(_mapper.Map(request.UpdateSolutionSummaryViewModel,
                await GetSolution(request, cancellationToken).ConfigureAwait(false)), cancellationToken).ConfigureAwait(false);

        private async Task<Solution> GetSolution(UpdateSolutionSummaryCommand request, CancellationToken cancellationToken) =>
            await _solutionReader.ByIdAsync(request.SolutionId, cancellationToken).ConfigureAwait(false);
    }
}
