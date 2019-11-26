using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.SubmitForReview
{
    public sealed class SubmitSolutionForReviewCommandResult
    {
        /// <summary>
        /// Gets a value to indicate whether or not this result was a failure.
        /// </summary>
        public bool IsFailure => Errors.Any();

        /// <summary>
        /// Gets a value to indicate whether or not this result was successful.
        /// </summary>
        public bool IsSuccess => !IsFailure;

        /// <summary>
        /// Gets the list of errors.
        /// </summary>
        public IReadOnlyCollection<ValidationError> Errors { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SubmitSolutionForReviewCommandResult"/> class.
        /// </summary>
        public SubmitSolutionForReviewCommandResult() : this(null)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SubmitSolutionForReviewCommandResult"/> class.
        /// </summary>
        public SubmitSolutionForReviewCommandResult(IReadOnlyCollection<ValidationError> errors)
        {
            Errors = errors ?? new ReadOnlyCollection<ValidationError>(new List<ValidationError>());
        }
    }
}
