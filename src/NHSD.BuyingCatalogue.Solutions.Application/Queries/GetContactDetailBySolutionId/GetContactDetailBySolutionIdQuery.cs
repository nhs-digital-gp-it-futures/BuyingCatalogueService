using System.Collections.Generic;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetContactDetailBySolutionId
{
    /// <summary>
    /// Represents the query parameters for the get Solution by ID request.
    /// </summary>
    public sealed class GetContactDetailBySolutionIdQuery : IRequest<IEnumerable<IContact>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetContactDetailBySolutionIdQuery"/> class.
        /// </summary>
        /// <param name="id">The solution ID.</param>
        public GetContactDetailBySolutionIdQuery(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the key information to identify a <see cref="Solution"/>.
        /// </summary>
        public string Id { get; }
    }
}
