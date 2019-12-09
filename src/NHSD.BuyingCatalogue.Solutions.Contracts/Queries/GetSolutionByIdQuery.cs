using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
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
