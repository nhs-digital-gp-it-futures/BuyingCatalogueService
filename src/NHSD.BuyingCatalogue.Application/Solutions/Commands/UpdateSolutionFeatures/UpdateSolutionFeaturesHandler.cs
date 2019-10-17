using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution
{
    internal sealed class UpdateSolutionFeaturesHandler : IRequestHandler<UpdateSolutionFeaturesCommand>
    {
        private readonly SolutionReader _solutionReader;
        private readonly SolutionFeaturesUpdater _solutionFeaturesUpdater;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionHandler"/> class.
        /// </summary>
        public UpdateSolutionFeaturesHandler(SolutionReader solutionReader, SolutionFeaturesUpdater solutionFeaturesUpdater, IMapper mapper)
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
        public async Task<Unit> Handle(UpdateSolutionFeaturesCommand request, CancellationToken cancellationToken)
        {
            Solution solution = await _solutionReader.ByIdAsync(request.SolutionId, cancellationToken);

            await _solutionFeaturesUpdater.UpdateAsync(_mapper.Map(request.UpdateSolutionFeaturesViewModel, solution), cancellationToken);

            return Unit.Value;
        }
    }
}
