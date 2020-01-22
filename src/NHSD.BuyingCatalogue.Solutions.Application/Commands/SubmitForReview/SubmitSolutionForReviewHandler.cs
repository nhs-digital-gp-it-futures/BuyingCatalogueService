using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

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
            Solution solution = await _solutionReader.ByIdAsync(request.SolutionId, cancellationToken).ConfigureAwait(false);

            ValidationResult validationResult = new SubmitSolutionForReviewValidator(solution).Validate();

            SubmitSolutionForReviewCommandResult result = new SubmitSolutionForReviewCommandResult(validationResult.Errors);
            if (result.IsSuccess)
            {
                await _solutionRepository.UpdateSupplierStatusAsync(new UpdateSolutionSupplierStatusRequest(solution.Id, SupplierStatus.AuthorityReview.Id), cancellationToken)
                    .ConfigureAwait(false);
            }

            return result;
        }
    }
}
