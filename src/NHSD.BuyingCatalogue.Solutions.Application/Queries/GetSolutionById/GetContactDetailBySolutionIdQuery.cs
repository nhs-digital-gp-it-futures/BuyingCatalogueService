using System.Collections.Generic;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    /// <summary>
    /// Represents the query parameters for the get Solution by ID request.
    /// </summary>
    public sealed class GetContactDetailBySolutionIdQuery : IRequest<IEnumerable<IContact>>
    {
        /// <summary>
        /// The key information to identify a <see cref="Solution"/>.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetSolutionByIdQuery"/> class.
        /// </summary>
        public GetContactDetailBySolutionIdQuery(string id)
        {
            Id = id;
        }
    }
}
