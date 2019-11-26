using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Contracts.Solutions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.SubmitForReview
{
    internal sealed class SubmitSolutionForReviewHandler : IRequestHandler<SubmitSolutionForReviewCommand, SubmitSolutionForReviewCommandResult>
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
        public async Task<SubmitSolutionForReviewCommandResult> Handle(SubmitSolutionForReviewCommand request, CancellationToken cancellationToken)
        {
            Solution solution = await GetSolution(request.SolutionId, cancellationToken);

            ValidationResult validationResult = new SubmitSolutionForReviewValidator(solution).Validate();

            SubmitSolutionForReviewCommandResult result = new SubmitSolutionForReviewCommandResult(validationResult.Errors);
            if (result.IsSuccess)
            {
                await _solutionRepository.UpdateSupplierStatusAsync(new UpdateSolutionSupplierStatusRequest(solution.Id, SupplierStatus.AuthorityReview.Id), cancellationToken);
            }

            return result;
        }

        /// <summary>
        /// Gets the details of the solution matching the specified ID.
        /// </summary>
        /// <param name="solutionId">The key information to identify a <see cref="Solution"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task representing an operation to retrieve a solution.</returns>
        private async Task<Solution> GetSolution(string solutionId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(solutionId))
            {
                throw new ArgumentException($"{nameof(solutionId)} cannot be null or empty.", nameof(solutionId));
            }

            return await _solutionReader.ByIdAsync(solutionId, cancellationToken);
        }
    }
}
