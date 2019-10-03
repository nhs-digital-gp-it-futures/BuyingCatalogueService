using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution
{
    public sealed class UpdateSolutionHandler : IRequestHandler<UpdateSolutionCommand>
    {
        private readonly ISolutionRepository _solutionsRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionHandler"/> class.
        /// </summary>
        public UpdateSolutionHandler(ISolutionRepository solutionRepository, IMapper mapper)
        {
            _solutionsRepository = solutionRepository ?? throw new System.ArgumentNullException(nameof(solutionRepository));
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
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
            Solution solution = await _solutionsRepository.ByIdAsync(solutionId, cancellationToken);
            if (solution is null)
            {
                throw new NotFoundException(nameof(Solution), solutionId);
            }

            Solution updatedSolution = _mapper.Map(updateSolutionViewModel, solution);

            await _solutionsRepository.UpdateAsync(updatedSolution, cancellationToken);

            return Unit.Value;
        }
    }
}
