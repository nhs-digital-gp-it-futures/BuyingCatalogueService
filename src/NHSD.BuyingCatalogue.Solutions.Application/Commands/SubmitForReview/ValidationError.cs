using System.Collections.Generic;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.SubmitForReview
{
    public sealed class ValidationError : ValueObject
    {
        /// <summary>
        /// Gets a value representing the ID of this instance.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        internal ValidationError(string id)
        {
            Id = id.ThrowIfNullOrWhitespace();
        }

        /// <inheritdoc />
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
