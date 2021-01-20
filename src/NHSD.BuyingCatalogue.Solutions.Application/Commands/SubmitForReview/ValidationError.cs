using System.Collections.Generic;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.SubmitForReview
{
    public sealed class ValidationError : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class with the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The validation error ID.</param>
        internal ValidationError(string id)
        {
            Id = id.ThrowIfNullOrWhitespace();
        }

        /// <summary>
        /// Gets a value representing the ID of this instance.
        /// </summary>
        public string Id { get; }

        /// <inheritdoc />
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
