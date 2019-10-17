using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview
{
    public class SolutionDescriptionSectionValidator
    {
        private readonly Solution _solution;

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionDescriptionSectionValidator"/> class.
        /// </summary>
        public SolutionDescriptionSectionValidator(Solution solution)
        {
            _solution = solution ?? throw new System.ArgumentNullException(nameof(solution));
        }

        internal SubmitSolutionForReviewResult Validate()
        {
            SubmitSolutionForReviewResult result = SubmitSolutionForReviewResult.Success;
            if (string.IsNullOrWhiteSpace(_solution.Summary))
            {
                result = SubmitSolutionForReviewResult.Failure.WithMissingSolutionDescriptionQuestion("summary");
            }

            return result;
        }
    }
}
