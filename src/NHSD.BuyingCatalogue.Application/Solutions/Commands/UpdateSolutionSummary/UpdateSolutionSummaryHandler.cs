using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution
{
    internal sealed class UpdateSolutionSummaryHandler : IRequestHandler<UpdateSolutionSummaryCommand>
    {
        private readonly SolutionReader _solutionReader;
        private readonly SolutionSummaryUpdater _solutionSummaryUpdater;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionHandler"/> class.
        /// </summary>
        public UpdateSolutionSummaryHandler(SolutionReader solutionReader, SolutionSummaryUpdater solutionSummaryUpdater, IMapper mapper)
        {
            _solutionReader = solutionReader;
            _solutionSummaryUpdater = solutionSummaryUpdater;
            _mapper = mapper;
        }

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="request">The command parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this command.</returns>
        public async Task<Unit> Handle(UpdateSolutionSummaryCommand request, CancellationToken cancellationToken)
        {
            Solution solution = await _solutionReader.ByIdAsync(request.SolutionId, cancellationToken);

            await _solutionSummaryUpdater.UpdateSummaryAsync(_mapper.Map(request.UpdateSolutionSummaryViewModel, solution), cancellationToken);

            return Unit.Value;
        }
    }
}
