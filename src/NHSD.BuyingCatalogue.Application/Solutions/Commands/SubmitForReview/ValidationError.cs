using System;
using System.Collections.Generic;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview
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
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
            }

            Id = id;
        }

        /// <inheritdoc />
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
