using MediatR;
using NHSD.BuyingCatalogue.Contracts.Solutions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    /// <summary>
    /// Represents the query parameters for the get Solution by ID request.
    /// </summary>
    public sealed class GetSolutionByIdQuery : IRequest<ISolution>
    {
        /// <summary>
        /// The key information to identify a <see cref="Solution"/>.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetSolutionByIdQuery"/> class.
        /// </summary>
        public GetSolutionByIdQuery(string id)
        {
            Id = id;
        }
    }
}
