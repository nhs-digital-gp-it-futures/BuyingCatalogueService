using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.SubmitForReview
{
    internal sealed class ValidationResult
    {
        private readonly List<ValidationError> errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        internal ValidationResult()
            : this(new List<ValidationError>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class
        /// with the specified <paramref name="errors"/>.
        /// </summary>
        /// <param name="errors">A collection of errors (can be <see langword="null"/>).</param>
        internal ValidationResult(List<ValidationError> errors)
        {
            this.errors = errors ?? throw new ArgumentNullException(nameof(errors));
            Errors = new ReadOnlyCollection<ValidationError>(this.errors);
        }

        /// <summary>
        /// Gets a read only list of errors.
        /// </summary>
        internal IReadOnlyCollection<ValidationError> Errors { get; }

        /// <summary>
        /// Gets a value indicating whether or not this instance contains any errors.
        /// </summary>
        internal bool IsValid => !Errors.Any();

        internal ValidationResult Add(ValidationError validationError)
        {
            if (validationError is null)
            {
                throw new ArgumentNullException(nameof(validationError));
            }

            errors.Add(validationError);
            return this;
        }

        internal ValidationResult Add(ValidationResult validationResult)
        {
            if (validationResult is null)
            {
                throw new ArgumentNullException(nameof(validationResult));
            }

            return Add(new[] { validationResult });
        }

        [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Params parameter can be null")]
        internal ValidationResult Add(params ValidationResult[] validationResults)
        {
            if (validationResults is not null)
            {
                errors.AddRange(validationResults.Where(x => x != null && !x.IsValid).SelectMany(x => x.Errors));
            }

            return this;
        }
    }
}
