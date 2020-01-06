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
        private readonly SolutionReader _solutionReader;
        private readonly SolutionFeaturesUpdater _solutionFeaturesUpdater;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionHandler"/> class.
        /// </summary>
        public UpdateSolutionFeaturesExecutor(SolutionReader solutionReader, SolutionFeaturesUpdater solutionFeaturesUpdater, IMapper mapper)
        {
            _solutionReader = solutionReader;
            _solutionFeaturesUpdater = solutionFeaturesUpdater;
            _mapper = mapper;
        }

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="request">The command parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this command.</returns>
        public async Task UpdateAsync(UpdateSolutionFeaturesCommand request, CancellationToken cancellationToken) =>
            await _solutionFeaturesUpdater.UpdateAsync(
                    _mapper.Map(request.Data,
                        await GetSolution(request, cancellationToken).ConfigureAwait(false)), cancellationToken)
                .ConfigureAwait(false);

        private async Task<Solution> GetSolution(UpdateSolutionFeaturesCommand request, CancellationToken cancellationToken) =>
            await _solutionReader.ByIdAsync(request.SolutionId, cancellationToken).ConfigureAwait(false);
    }
}
