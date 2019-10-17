using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview
{
    /// <summary>
    /// Provides the result of <see cref="SubmitSolutionForReviewCommand"/>.
    /// </summary>
    public sealed class SubmitSolutionForReviewResult
    {
        public static readonly SubmitSolutionForReviewResult Success = new SubmitSolutionForReviewResult();
        public static readonly SubmitSolutionForReviewResult Failure = new SubmitSolutionForReviewResult(true);

        [JsonProperty("solution-description")]
        public SolutionDescriptionSectionValidationResultViewModel SolutionDescription { get; private set; }

        [JsonIgnore]
        public bool IsSuccess => !IsFailure;

        [JsonIgnore]
        private bool IsFailure { get; }

        private SubmitSolutionForReviewResult()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SubmitSolutionForReviewResult"/> class.
        /// </summary>
        private SubmitSolutionForReviewResult(bool isFailure)
        {
            IsFailure = isFailure;
        }

        private void EnsureSolutionDescriptionIsInitialised()
        {
            if (SolutionDescription is null)
            {
                SolutionDescription = new SolutionDescriptionSectionValidationResultViewModel();
            }
        }

        internal SubmitSolutionForReviewResult WithMissingSolutionDescriptionQuestion(string questionId)
        {
            if (string.IsNullOrWhiteSpace(questionId))
            {
                throw new System.ArgumentException("Cannot be null or empty.", nameof(questionId));
            }

            EnsureSolutionDescriptionIsInitialised();

            SolutionDescription.AddMissingQuestion(questionId);

            return this;
        }
    }
}
