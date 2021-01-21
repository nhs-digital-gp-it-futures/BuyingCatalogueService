using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.SubmitForReview
{
    public sealed class SubmitSolutionForReviewCommand : IRequest<SubmitSolutionForReviewCommandResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitSolutionForReviewCommand"/> class with the given ID.
        /// </summary>
        /// <param name="solutionId">The solution ID.</param>
        public SubmitSolutionForReviewCommand(string solutionId)
        {
            SolutionId = solutionId.ThrowIfNullOrWhitespace();
        }

        /// <summary>
        /// Gets a v value to uniquely identify a solution.
        /// </summary>
        public string SolutionId { get; }
    }
}
