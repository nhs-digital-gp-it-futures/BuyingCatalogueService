using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview
{
    public sealed class SubmitSolutionForReviewHandler : IRequestHandler<SubmitSolutionForReviewCommand>
    {
        private readonly ISolutionRepository _solutionsRepository;

        /// <summary>
        /// Initialises a new instance of the <see cref="SubmitSolutionForReviewHandler"/> class.
        /// </summary>
        public SubmitSolutionForReviewHandler(ISolutionRepository solutionRepository)
        {
            _solutionsRepository = solutionRepository ?? throw new ArgumentNullException(nameof(solutionRepository));
        }

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="request">The command parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this command.</returns>
        public async Task<Unit> Handle(SubmitSolutionForReviewCommand request, CancellationToken cancellationToken)
        {
            Solution solution = await GetSolution(request.SolutionId, cancellationToken);

            await _solutionsRepository.UpdateSupplierStatusAsync(solution, SupplierStatus.AuthorityReview, cancellationToken);

            return Unit.Value;
        }

        /// <summary>
        /// Gets the details of the solution matching the specified ID.
        /// </summary>
        /// <param name="solutionId">The key information to identfy a <see cref="Solution"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<Solution> GetSolution(string solutionId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(solutionId))
            {
                throw new ArgumentException($"{nameof(solutionId)} cannot be null or empty.", nameof(solutionId));
            }

            Solution solution = await _solutionsRepository.ByIdAsync(solutionId, cancellationToken);
            return solution ?? throw new NotFoundException(nameof(Solution), solutionId);
        }
    }
}
