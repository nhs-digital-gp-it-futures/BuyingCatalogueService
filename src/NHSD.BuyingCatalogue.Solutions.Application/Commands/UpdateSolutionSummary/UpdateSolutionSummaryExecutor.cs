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
        private readonly SolutionReader solutionReader;
        private readonly SolutionSummaryUpdater solutionSummaryUpdater;
        private readonly IMapper mapper;

        public UpdateSolutionSummaryExecutor(
            SolutionReader solutionReader,
            SolutionSummaryUpdater solutionSummaryUpdater,
            IMapper mapper)
        {
            this.solutionReader = solutionReader;
            this.solutionSummaryUpdater = solutionSummaryUpdater;
            this.mapper = mapper;
        }

        public async Task UpdateAsync(UpdateSolutionSummaryCommand request, CancellationToken cancellationToken) =>
            await solutionSummaryUpdater.UpdateSummaryAsync(
                mapper.Map(request.Data, await GetSolution(request, cancellationToken)),
                cancellationToken);

        private async Task<Solution> GetSolution(UpdateSolutionSummaryCommand request, CancellationToken cancellationToken) =>
            await solutionReader.ByIdAsync(request.SolutionId, cancellationToken);
    }
}
