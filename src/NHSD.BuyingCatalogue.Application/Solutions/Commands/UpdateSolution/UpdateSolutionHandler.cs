using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution
{
    internal sealed class UpdateSolutionHandler : IRequestHandler<UpdateSolutionCommand>
    {
        private readonly SolutionReader _solutionReader;
        private readonly SolutionUpdater _solutionUpdater;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionHandler"/> class.
        /// </summary>
        public UpdateSolutionHandler(SolutionReader solutionReader, SolutionUpdater solutionUpdater, IMapper mapper)
        {
            _solutionReader = solutionReader;
            _solutionUpdater = solutionUpdater;
            _mapper = mapper;
        }

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="request">The command parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this command.</returns>
        public async Task<Unit> Handle(UpdateSolutionCommand request, CancellationToken cancellationToken)
        {
            UpdateSolutionViewModel updateSolutionViewModel = request.UpdateSolutionViewModel;

            string solutionId = request.SolutionId;
            Solution solution = await _solutionReader.ByIdAsync(solutionId, cancellationToken);
            if (solution is null)
            {
                throw new NotFoundException(nameof(Solution), solutionId);
            }

            Solution updatedSolution = _mapper.Map(updateSolutionViewModel, solution);

            await _solutionUpdater.UpdateAsync(updatedSolution, cancellationToken);

            return Unit.Value;
        }
    }
}
