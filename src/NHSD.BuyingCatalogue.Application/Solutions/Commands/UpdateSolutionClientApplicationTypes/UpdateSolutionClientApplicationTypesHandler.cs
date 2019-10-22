using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution
{
    internal sealed class UpdateSolutionClientApplicationTypesHandler : IRequestHandler<UpdateSolutionClientApplicationTypesCommand>
    {
        private readonly SolutionReader _solutionReader;
        private readonly SolutionClientApplicationTypesUpdater _solutionClientApplicationTypesUpdater;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionHandler"/> class.
        /// </summary>
        public UpdateSolutionClientApplicationTypesHandler(SolutionReader solutionReader, SolutionClientApplicationTypesUpdater solutionClientApplicationTypesUpdater, IMapper mapper)
        {
            _solutionReader = solutionReader;
            _solutionClientApplicationTypesUpdater = solutionClientApplicationTypesUpdater;
            _mapper = mapper;
        }

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="request">The command parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this command.</returns>
        public async Task<Unit> Handle(UpdateSolutionClientApplicationTypesCommand request, CancellationToken cancellationToken)
        {
            Solution solution = await _solutionReader.ByIdAsync(request.SolutionId, cancellationToken);

            await _solutionClientApplicationTypesUpdater.UpdateAsync(Map(request.UpdateSolutionClientApplicationTypesViewModel), request.SolutionId, cancellationToken);

            return Unit.Value;
        }

        private static ClientApplication Map(UpdateSolutionClientApplicationTypesViewModel updateSolutionClientApplicationTypesViewModel) =>
            new ClientApplication
            {
                ClientApplicationTypes = updateSolutionClientApplicationTypesViewModel.ClientApplicationTypes
            };
    }
}
