using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview
{
    internal class ValidationResult
    {
        private readonly List<ValidationError> _errors;

        /// <summary>
        /// Gets a read only list of errors.
        /// </summary>
        internal IReadOnlyCollection<ValidationError> Errors { get; }

        /// <summary>
        /// Gets a value to indicate whether or not this instance contains any errors.
        /// </summary>
        internal bool IsValid => !Errors.Any();

        /// <summary>
        /// Initialises a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        internal ValidationResult() : this(new List<ValidationError>())
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        internal ValidationResult(List<ValidationError> errors)
        {
            _errors = errors ?? throw new ArgumentNullException(nameof(errors));
            Errors = new ReadOnlyCollection<ValidationError>(_errors);
        }

        internal ValidationResult Add(ValidationError validationError)
        {
            _errors.Add(validationError.ThrowIfNull(nameof(validationError)));
            return this;
        }

        internal ValidationResult Add(ValidationResult validationResult)
        {
            return Add(new[] {validationResult.ThrowIfNull(nameof(validationResult)) });
        }

        internal ValidationResult Add(params ValidationResult[] validationResults)
        {
            if (validationResults is object)
            {
                _errors.AddRange(validationResults.Where(x => x != null && !x.IsValid).SelectMany(x => x.Errors));
            }

            return this;
        }
    }
}
