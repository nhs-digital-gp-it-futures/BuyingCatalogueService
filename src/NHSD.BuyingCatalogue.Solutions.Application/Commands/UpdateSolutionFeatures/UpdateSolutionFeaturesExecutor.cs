using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures
{
    internal sealed class UpdateSolutionFeaturesExecutor : IExecutor<UpdateSolutionFeaturesCommand>
    {
        private readonly SolutionReader solutionReader;
        private readonly SolutionFeaturesUpdater solutionFeaturesUpdater;
        private readonly IMapper mapper;

        public UpdateSolutionFeaturesExecutor(
            SolutionReader solutionReader,
            SolutionFeaturesUpdater solutionFeaturesUpdater,
            IMapper mapper)
        {
            this.solutionReader = solutionReader;
            this.solutionFeaturesUpdater = solutionFeaturesUpdater;
            this.mapper = mapper;
        }

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="request">The command parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this command.</returns>
        public async Task UpdateAsync(UpdateSolutionFeaturesCommand request, CancellationToken cancellationToken) =>
            await solutionFeaturesUpdater.UpdateAsync(
                mapper.Map(request.Data, await GetSolution(request, cancellationToken)),
                cancellationToken);

        private async Task<Solution> GetSolution(UpdateSolutionFeaturesCommand request, CancellationToken cancellationToken) =>
            await solutionReader.ByIdAsync(request.SolutionId, cancellationToken);
    }
}
