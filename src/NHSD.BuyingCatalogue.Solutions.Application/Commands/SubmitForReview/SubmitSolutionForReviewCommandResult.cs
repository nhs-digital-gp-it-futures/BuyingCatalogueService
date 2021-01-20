using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.SubmitForReview
{
    public sealed class SubmitSolutionForReviewCommandResult
    {
        /// <summary>
        /// Gets a value indicating whether or not this result was a failure.
        /// </summary>
        public bool IsFailure => Errors.Any();

        /// <summary>
        /// Gets a value indicating whether or not this result was successful.
        /// </summary>
        public bool IsSuccess => !IsFailure;

        /// <summary>
        /// Gets the list of errors.
        /// </summary>
        public IReadOnlyCollection<ValidationError> Errors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitSolutionForReviewCommandResult"/> class.
        /// </summary>
        public SubmitSolutionForReviewCommandResult()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitSolutionForReviewCommandResult"/> class
        /// with the specified <paramref name="errors"/>.
        /// </summary>
        /// <param name="errors">A collection of errors (can be <see langword="null"/>).</param>
        public SubmitSolutionForReviewCommandResult(IReadOnlyCollection<ValidationError> errors)
        {
            Errors = errors ?? new ReadOnlyCollection<ValidationError>(new List<ValidationError>());
        }
    }
}
