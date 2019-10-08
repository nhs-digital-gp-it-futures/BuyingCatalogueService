using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview
{
    internal sealed class SubmitSolutionForReviewHandler : IRequestHandler<SubmitSolutionForReviewCommand>
    {
        private readonly SolutionReader _solutionReader;
        private readonly ISolutionRepository _solutionRepository;

        /// <summary>
        /// Initialises a new instance of the <see cref="SubmitSolutionForReviewHandler"/> class.
        /// </summary>
        public SubmitSolutionForReviewHandler(SolutionReader solutionReader, ISolutionRepository solutionRepository)
        {
            _solutionReader = solutionReader;
            _solutionRepository = solutionRepository;
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

            await _solutionRepository.UpdateSupplierStatusAsync(new UpdateSolutionSupplierStatusRequest { Id = solution.Id, SupplierStatusId = SupplierStatus.AuthorityReview.Id }, cancellationToken);

            return Unit.Value;
        }

        /// <summary>
        /// Gets the details of the solution matching the specified ID.
        /// </summary>
        /// <param name="solutionId">The key information to identfy a <see cref="Solution"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task representing an operation to retrieve a solution.</returns>
        private async Task<Solution> GetSolution(string solutionId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(solutionId))
            {
                throw new ArgumentException($"{nameof(solutionId)} cannot be null or empty.", nameof(solutionId));
            }

            Solution solution = await _solutionReader.ByIdAsync(solutionId, cancellationToken);
            return solution ?? throw new NotFoundException(nameof(Solution), solutionId);
        }
    }
}
