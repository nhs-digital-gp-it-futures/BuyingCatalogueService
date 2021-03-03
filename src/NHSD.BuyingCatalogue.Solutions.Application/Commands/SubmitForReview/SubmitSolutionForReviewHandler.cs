﻿using System.Threading;
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
        private readonly SolutionReader solutionReader;
        private readonly ISolutionRepository solutionRepository;

        public SubmitSolutionForReviewHandler(SolutionReader solutionReader, ISolutionRepository solutionRepository)
        {
            this.solutionReader = solutionReader;
            this.solutionRepository = solutionRepository;
        }

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="request">The command parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this command.</returns>
        public async Task<SubmitSolutionForReviewCommandResult> Handle(
            SubmitSolutionForReviewCommand request,
            CancellationToken cancellationToken)
        {
            Solution solution = await solutionReader.ByIdAsync(request.SolutionId, cancellationToken);

            ValidationResult validationResult = new SubmitSolutionForReviewValidator(solution).Validate();

            SubmitSolutionForReviewCommandResult result = new SubmitSolutionForReviewCommandResult(validationResult.Errors);
            return result;
        }
    }
}
